using Reservico.Common.Models;
using Reservico.Services.Dashboard.Models;

namespace Reservico.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<ServiceResponse<DashboardViewModel>> Get(
            Guid? clientId);
    }
}