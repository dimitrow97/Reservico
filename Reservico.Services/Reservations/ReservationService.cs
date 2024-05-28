using Microsoft.EntityFrameworkCore;
using Reservico.Common.EmailSender;
using Reservico.Common.Models;
using Reservico.Data.Entities;
using Reservico.Data.Interfaces;
using Reservico.Services.Locations;
using Reservico.Services.Reservations.Models;

namespace Reservico.Services.Reservations
{
    public class ReservationService : IReservationService
    {
        private readonly IEmailSender emailSender;
        private readonly ILocationService locationService;
        private readonly IRepository<Table> tableRepository;
        private readonly IRepository<Reservation> reservationRepository;

        public ReservationService(
            IEmailSender emailSender,
            ILocationService locationService,
            IRepository<Table> tableRepository,
            IRepository<Reservation> reservationRepository)
        {
            this.emailSender = emailSender;
            this.locationService = locationService;
            this.tableRepository = tableRepository;
            this.reservationRepository = reservationRepository;
        }

        public async Task<ServiceResponse<IEnumerable<ReservationViewModel>>> GetAll(
            Guid? clientId)
        {
            var reservationsQuery = this.reservationRepository
                .Query()
                .Include(x => x.Table)
                    .ThenInclude(x => x.Location)
                        .ThenInclude(x => x.Client)
                .Where(x => !x.Table.IsDeleted && !x.Table.Location.IsDeleted && !x.Table.Location.Client.IsDeleted);

            List<Reservation> reservations;

            if (clientId.HasValue)
            {
                reservations = await reservationsQuery
                    .Where(x => x.Table.Location.ClientId.Equals(clientId.Value))
                    .OrderBy(x => x.GuestsArrivingAt < DateTime.UtcNow.Date)
                        .ThenBy(x => x.GuestsArrivingAt)
                    .ToListAsync();
            }
            else
            {
                reservations = await reservationsQuery
                    .OrderBy(x => x.GuestsArrivingAt < DateTime.UtcNow.Date)
                         .ThenBy(x => x.GuestsArrivingAt)
                    .ToListAsync();
            }

            return ServiceResponse<IEnumerable<ReservationViewModel>>.Success(
                reservations.Select(x => new ReservationViewModel
                {
                    Id = x.Id,
                    GuestsArrivingAt = x.GuestsArrivingAt,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    IsConfirmed = x.IsConfirmed,
                    Note = x.Note,
                    NumberOfGuests = x.NumberOfGuests,
                    IsDeleted = x.IsDeleted,
                    LocationId = x.Table.LocationId,
                    LocationName = x.Table.Location.Name,
                    TableId = x.TableId,
                    TableName = x.Table.Name
                }));
        }

        public async Task<ServiceResponse<ReservationViewModel>> Get(
            Guid reservationId)
        {
            var reservation = await this.reservationRepository
                .Query()
                .Include(x => x.Table)
                    .ThenInclude(x => x.Location)
                .FirstOrDefaultAsync(x => x.Id.Equals(reservationId));

            if (reservation is null)
            {
                return ServiceResponse<ReservationViewModel>.Error(
                    "Reservation does NOT exists!");
            }

            var result = new ReservationViewModel
            {
                Id = reservation.Id,
                GuestsArrivingAt = reservation.GuestsArrivingAt,
                FirstName = reservation.FirstName,
                LastName = reservation.LastName,
                Email = reservation.Email,
                PhoneNumber = reservation.PhoneNumber,
                IsConfirmed = reservation.IsConfirmed,
                Note = reservation.Note,
                NumberOfGuests = reservation.NumberOfGuests,
                IsDeleted = reservation.IsDeleted,
                LocationId = reservation.Table.LocationId,
                LocationName = reservation.Table.Location.Name,
                TableId = reservation.TableId,
                TableName = reservation.Table.Name
            };

            return ServiceResponse<ReservationViewModel>
                .Success(result);
        }

        public async Task<ServiceResponse> MakeReservation(
            MakeReservationRequestModel model)
        {
            var locationExists = await this.locationService
                .LocationExists(model.LocationId);

            if (!locationExists.IsSuccess)
            {
                return ServiceResponse.Error(
                    locationExists.ErrorMessage);
            }

            var checkTables = await this.CheckForAFreeTable(
                model.LocationId,
                model.GuestsArrivingAt,
                model.NumberOfGuests);

            if (!checkTables.IsSuccess)
            {
                return ServiceResponse.Error(checkTables.ErrorMessage);
            }

            var reservation = new Reservation
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Note = model.Note,
                GuestsArrivingAt = model.GuestsArrivingAt,
                NumberOfGuests = model.NumberOfGuests,
                TableId = checkTables.Data,
                CreatedOn = DateTime.UtcNow,
                IsConfirmed = false
            };

            await this.reservationRepository.AddAsync(reservation);

            var location = await this.locationService.Get(
                model.LocationId);

            await this.emailSender.ReservationCreatedEmail(
                reservation.Email,
                reservation.GuestsArrivingAt,
                reservation.NumberOfGuests,
                location.Data.Name);

            await this.emailSender.ReservationEmailToLocation(
                location.Data.Email,
                reservation.GuestsArrivingAt,
                reservation.NumberOfGuests,
                location.Data.Name);

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse> ReservationExists(Guid reservationId)
        {
            var result = await this.reservationRepository.Query()
                .Where(c => !c.IsDeleted).AnyAsync(x => x.Id == reservationId);

            if (result == false)
            {
                return ServiceResponse.Error("Reservation does NOT exist");
            }

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse> Confirm(Guid reservationId)
        {
            var reservationExists = await this.ReservationExists(reservationId);

            if (!reservationExists.IsSuccess)
            {
                return ServiceResponse.Error(
                    reservationExists.ErrorMessage);
            }

            var reservation = await this.reservationRepository
                .Query()
                .Include(x => x.Table)
                    .ThenInclude(x => x.Location)
                .FirstOrDefaultAsync(x => x.Id.Equals(reservationId));

            var check = await CheckIfTableAlreadyTaken(reservation);

            if (!check.IsSuccess)
            {
                return ServiceResponse.Error(
                    check.ErrorMessage);
            }

            reservation.UpdatedOn = DateTime.UtcNow;
            reservation.IsConfirmed = true;

            await this.reservationRepository.UpdateAsync(reservation);

            await this.emailSender.ReservationConfirmedEmail(
                reservation.Email,
                reservation.GuestsArrivingAt,
                reservation.NumberOfGuests,
                reservation.Table.Location.Name);

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse> Cancel(Guid reservationId)
        {
            var reservationExists = await this.ReservationExists(reservationId);

            if (!reservationExists.IsSuccess)
            {
                return ServiceResponse.Error(
                    reservationExists.ErrorMessage);
            }

            var reservation = await this.reservationRepository
                .Query()
                .Include(x => x.Table)
                    .ThenInclude(x => x.Location)
                .FirstOrDefaultAsync(x => x.Id.Equals(reservationId));

            if (reservation.IsConfirmed)
            {
                return ServiceResponse.Error(
                    "Reservation is already confirmed. Please contact the establishment and try to cancel.");
            }

            reservation.UpdatedOn = DateTime.UtcNow;
            reservation.IsConfirmed = false;
            reservation.IsDeleted = true;

            await this.reservationRepository.UpdateAsync(reservation);

            await this.emailSender.ReservationCancelledEmail(
               reservation.Email,
               reservation.GuestsArrivingAt,
               reservation.NumberOfGuests,
               reservation.Table.Location.Name);

            await this.emailSender.ReservationCancelledEmailToLocation(
               reservation.Table.Location.Email,
               reservation.GuestsArrivingAt,
               reservation.NumberOfGuests,
               reservation.Table.Location.Name);

            return ServiceResponse.Success();
        }

        private async Task<ServiceResponse> CheckIfTableAlreadyTaken(
            Reservation reservation)
        {
            var isTableAlreadyTaken = await this.reservationRepository
                .Query()
                .Include(x => x.Table)
                .Where(x => x.IsConfirmed && !x.IsDeleted)
                .Where(x => x.TableId.Equals(reservation.TableId))
                .Where(x => x.GuestsArrivingAt.Equals(reservation.GuestsArrivingAt) ||
                    (x.GuestsArrivingAt.Date.Equals(reservation.GuestsArrivingAt.Date) &&
                        (x.GuestsArrivingAt.AddHours(x.Table.TableTurnOffset) <= reservation.GuestsArrivingAt ||
                         x.GuestsArrivingAt.AddHours((x.Table.TableTurnOffset * -1)) >= reservation.GuestsArrivingAt)))
                .FirstOrDefaultAsync();

            if (isTableAlreadyTaken is not null) 
            {
                return ServiceResponse.Error("A Reservation has been already confirmed for this table, date and time.");
            }

            return ServiceResponse.Success();
        }

        private async Task<ServiceResponse<Guid>> CheckForAFreeTable(
            Guid locationId,
            DateTime desiredTime,
            int numberOfGuests)
        {
            var tables = await this.tableRepository
                .Query()
                .Include(x => x.Reservations)
                .Where(x => x.LocationId.Equals(locationId))
                .ToListAsync();

            if (tables is null)
            {
                return ServiceResponse<Guid>.Error("Location does NOT have any Tables");
            }

            var table = tables
                .Where(x => x.Capacity >= numberOfGuests)
                .Where(x =>
                    (!x.CanTableTurn &&
                        x.Reservations.Any(y => y.GuestsArrivingAt.Date.Equals(desiredTime.Date) && !y.IsConfirmed)) ||
                    (x.CanTableTurn &&
                        x.Reservations.Any(y => (!y.IsConfirmed &&
                            (y.GuestsArrivingAt.Date.Equals(desiredTime.Date) ||
                            (y.GuestsArrivingAt.Equals(desiredTime))))
                        ) ||
                        x.Reservations.Any(y => y.IsConfirmed &&
                            y.GuestsArrivingAt.Date.Equals(desiredTime.Date) &&
                                (y.GuestsArrivingAt.AddHours(x.TableTurnOffset) <= desiredTime ||
                                y.GuestsArrivingAt.AddHours((x.TableTurnOffset * -1)) >= desiredTime))
                     ))
                .OrderBy(x => x.Reservations.Count)
                .FirstOrDefault();

            if (table is null)
            {
                return ServiceResponse<Guid>.Error("There are NO free Tables at the desired location and time");
            }

            return ServiceResponse<Guid>.Success(table.Id);
        }
    }
}