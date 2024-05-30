using Reservico.Services.Reservations.Models;

namespace Reservico.Services.Dashboard.Models
{
    public class DashboardViewModel
    {
        public int TotalNumberOfConfirmedReservations { get; set; }
        public int TotalNumberOfLocations { get; set; }
        public int TotalNumberOfReservations { get; set; }
        public double PercentMoreConfirmedReservations { get; set; }
        public double PercentMoreReservations { get; set; }

        public IEnumerable<ReservationViewModel> LastFiveReservations { get; set; }
    }
}
