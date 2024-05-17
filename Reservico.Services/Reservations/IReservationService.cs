using Reservico.Common.Models;
using Reservico.Services.Reservations.Models;

namespace Reservico.Services.Reservations
{
    public interface IReservationService
    {
        Task<ServiceResponse> MakeReservation(
            MakeReservationRequestModel model);

        Task<ServiceResponse> ReservationExists(Guid reservationId);

        Task<ServiceResponse> Confirm(Guid reservationId);

        Task<ServiceResponse> Cancel(Guid reservationId);
    }
}