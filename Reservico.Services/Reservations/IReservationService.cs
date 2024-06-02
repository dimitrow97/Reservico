using Reservico.Common.Models;
using Reservico.Services.Reservations.Models;

namespace Reservico.Services.Reservations
{
    public interface IReservationService
    {
        Task<ServiceResponse<IEnumerable<ReservationViewModel>>> GetAll(
            Guid? locationId);

        Task<ServiceResponse<ReservationViewModel>> Get(
            Guid reservationId);

        Task<ServiceResponse> MakeReservation(
            MakeReservationRequestModel model);

        Task<ServiceResponse> ReservationExists(Guid reservationId);

        Task<ServiceResponse> Confirm(Guid reservationId);

        Task<ServiceResponse> SendCancellationEmail(Guid reservationId);

        Task<ServiceResponse> Cancel(Guid reservationId);

        Task<ServiceResponse<ReservationsReportViewModel>> GetReservationsReport(
            Guid? clientId);
    }
}