using Reservico.Common.Models;
using Reservico.Data.Entities;
using Reservico.Data.Interfaces;
using Reservico.Services.Dashboard.Models;
using Reservico.Services.Reservations;

namespace Reservico.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly IReservationService reservationService;
        private readonly IRepository<Location> locationRepository;

        public DashboardService(
            IReservationService reservationService,
            IRepository<Location> locationRepository) 
        {
            this.reservationService = reservationService;
            this.locationRepository = locationRepository;
        }

        public async Task<ServiceResponse<DashboardViewModel>> Get(
            Guid? clientId)
        {
            var locations = this.locationRepository
                .Query()
                .Where(x => !x.IsDeleted);

            if (clientId.HasValue)
            {
                locations = locations
                    .Where(x => x.ClientId.Equals(clientId.Value));
            }

            var totalNumberOfLocations = locations.Count();

            var reservationsReport = await this.reservationService
                .GetReservationsReport(clientId);

            var dashboardVm = new DashboardViewModel 
            { 
                TotalNumberOfLocations = totalNumberOfLocations,
                TotalNumberOfReservations = reservationsReport.Data.TotalNumberOfReservations,
                TotalNumberOfConfirmedReservations = reservationsReport.Data.TotalNumberOfConfirmedReservations,
                PercentMoreReservations = reservationsReport.Data.PercentMoreReservations,
                PercentMoreConfirmedReservations = reservationsReport.Data.PercentMoreConfirmedReservations,
                LastFiveReservations = reservationsReport.Data.LastFiveReservations
            };

            return ServiceResponse<DashboardViewModel>.Success(dashboardVm);
        }

        
    }
}
