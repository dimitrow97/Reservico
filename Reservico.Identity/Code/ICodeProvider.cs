using Reservico.Common.Models;
using Reservico.Data.Entities.Abstraction;
using Reservico.Identity.Code.Models;

namespace Reservico.Identity.Code
{
    public interface ICodeProvider<TCode>
        where TCode : BaseCode
    {
        Task<string> GenerateCodeAsync(GenerateCodeRequestModel model);

        Task<ServiceResponse<ValidateCodeResponseModel>> ValidateCodeAsync(
            string code);

        Task MarkCodeAsUsed(string code);

        Task<ServiceResponse<string>> RefreshCode(RefreshCodeRequestModel model);
    }
}
