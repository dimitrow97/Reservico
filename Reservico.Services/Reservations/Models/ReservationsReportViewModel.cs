namespace Reservico.Services.Reservations.Models
{
    public class ReservationsReportViewModel
    {
        public int TotalNumberOfConfirmedReservations { get; set; }
        public int TotalNumberOfReservations { get; set; }
        public double PercentMoreConfirmedReservations { get; set; }
        public double PercentMoreReservations { get; set; }

        public IEnumerable<ReservationViewModel> LastFiveReservations { get; set; }
    }
}