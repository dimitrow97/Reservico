using Microsoft.EntityFrameworkCore;
using Reservico.Common.Models;
using Reservico.Data.Entities;
using Reservico.Data.Interfaces;
using Reservico.Services.Locations;
using Reservico.Services.Reservations.Models;

namespace Reservico.Services.Reservations
{
    public class ReservationService : IReservationService
    {
        private readonly ILocationService locationService;
        private readonly IRepository<Reservation> reservationRepository;

        public ReservationService(
            ILocationService locationService,
            IRepository<Reservation> reservationRepository) 
        {
            this.locationService = locationService;
            this.reservationRepository = reservationRepository;
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

            //send email for created reservation

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
                .GetByIdAsync(reservationId);

            reservation.UpdatedOn = DateTime.UtcNow;
            reservation.IsConfirmed = true;

            await this.reservationRepository.UpdateAsync(reservation);

            //send email to Confirm

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
                .GetByIdAsync(reservationId);

            if (reservation.IsConfirmed)
            {
                return ServiceResponse.Error(
                    "Reservation is already confirmed. Please contact the establishment and try to cancel.");
            }

            reservation.UpdatedOn = DateTime.UtcNow;
            reservation.IsConfirmed = false;
            reservation.IsDeleted = true;

            await this.reservationRepository.UpdateAsync(reservation);

            //send email to Cancel

            return ServiceResponse.Success();
        }

        private async Task<ServiceResponse<Guid>> CheckForAFreeTable(
            Guid locationId,
            DateTime desiredTime,
            int numberOfGuests)
        {
            var tables = await this.locationService.GetLocationTables(
                locationId);

            if (tables is null)
            {
                return ServiceResponse<Guid>.Error("Location does NOT have any Tables");
            }

            var table = tables.Data                
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