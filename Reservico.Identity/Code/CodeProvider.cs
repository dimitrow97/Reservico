using Reservico.Common.Models;
using Reservico.Data.Entities;
using Reservico.Data.Interfaces;
using Reservico.Identity.Code.Models;
using System.Text;

namespace Reservico.Identity.Code
{
    public class CodeProvider<TCode> : ICodeProvider<TCode>
        where TCode : IdentityAuthorizationCode, new()
    {
        protected readonly IRepository<TCode> repository;

        public CodeProvider(
            IRepository<TCode> repository)
        {
            this.repository = repository;
        }

        public virtual async Task<string> GenerateCodeAsync(GenerateCodeRequestModel model)
        {
            var utcNow = DateTime.UtcNow;

            var codeEntity = new TCode
            {
                ClientId = model.ClientId,
                IsUsed = false,
                CreatedOn = utcNow
            };

            SetCodeExpiration(codeEntity, utcNow, model.CodeExpirationTime, model.ExpirationTimeType);

            codeEntity.Code = GenerateCode(codeEntity, model.CodeValues);

            await repository.AddAsync(codeEntity);

            return codeEntity.Code;
        }

        public virtual async Task<ServiceResponse<ValidateCodeResponseModel>> ValidateCodeAsync(
            string code)
        {
            var codeEntity = await repository
                .FindByConditionAsync(x => x.Code.Equals(code));

            if (codeEntity is null)
            {
                return ServiceResponse<ValidateCodeResponseModel>
                    .Error("Invalid code.");
            }

            if (codeEntity.IsUsed)
            {
                return ServiceResponse<ValidateCodeResponseModel>
                    .Error("The code has already been used.");
            }

            if (codeEntity.ExpirationDate < DateTime.UtcNow)
            {
                return ServiceResponse<ValidateCodeResponseModel>
                    .Error("Code has expired.");
            }

            var exraction = ExtractExpirationAndCodeValues(code);

            if (exraction.Expiration < DateTime.UtcNow)
            {
                return ServiceResponse<ValidateCodeResponseModel>
                    .Error("Code has expired.");
            }

            return ServiceResponse<ValidateCodeResponseModel>.Success(
                new ValidateCodeResponseModel(codeEntity.ClientId, exraction.CodeValues));
        }

        public virtual async Task MarkCodeAsUsed(string code)
        {
            var codeEntity = await repository
                .FindByConditionAsync(x => x.Code.Equals(code));

            if (codeEntity is null)
            {
                throw new ArgumentNullException("Code not found.");
            }

            codeEntity.IsUsed = true;
            codeEntity.UpdatedOn = DateTime.UtcNow;

            await repository.UpdateAsync(codeEntity);
        }

        public virtual async Task<ServiceResponse<string>> RefreshCode(RefreshCodeRequestModel model)
        {
            var utcNow = DateTime.UtcNow;

            var codeEntity = await repository.FindByConditionAsync(x => x.Code.Equals(model.Code));

            if (codeEntity is null)
            {
                return ServiceResponse<string>.Error("Invalid code.");
            }

            this.SetCodeExpiration(codeEntity, utcNow, model.CodeExpirationTime, model.ExpirationTimeType);

            var extraction = ExtractExpirationAndCodeValues(model.Code);

            codeEntity.Code = this.GenerateCode(codeEntity, extraction.CodeValues);
            codeEntity.UpdatedOn = utcNow;

            await repository.UpdateAsync(codeEntity);

            return ServiceResponse<string>.Success(codeEntity.Code);
        }

        protected virtual string GenerateCode(TCode codeEntity, IEnumerable<string> codeValues)
        {
            var time = BitConverter.GetBytes(codeEntity.ExpirationDate.ToBinary());
            var codeValue = Encoding.ASCII.GetBytes(GenerateCodeValue(codeValues));
            var data = new byte[time.Length + codeValue.Length];

            Buffer.BlockCopy(time, 0, data, 0, time.Length);
            Buffer.BlockCopy(codeValue, 0, data, time.Length, codeValue.Length);

            return Convert.ToBase64String(data);
        }

        protected virtual string GenerateCodeValue(IEnumerable<string> codeValues)
            => string.Join("/", codeValues);

        protected virtual (DateTime Expiration, IEnumerable<string> CodeValues) ExtractExpirationAndCodeValues(
            string code)
        {
            var data = Convert.FromBase64String(code);
            var expiration = data.Take(8).ToArray();
            var codeValueBytes = data.Skip(8).ToArray();

            return (DateTime.FromBinary(BitConverter.ToInt64(expiration, 0)),
                Encoding.ASCII.GetString(codeValueBytes).Split("/"));
        }

        private void SetCodeExpiration(
            TCode codeEntity,
            DateTime utcNow,
            int codeExpirationTime,
            ExpirationTimeType expirationTimeType)
        {
            switch (expirationTimeType)
            {
                case ExpirationTimeType.Seconds:
                    codeEntity.ExpirationDate = utcNow.AddSeconds(codeExpirationTime);
                    break;
                case ExpirationTimeType.Minutes:
                    codeEntity.ExpirationDate = utcNow.AddMinutes(codeExpirationTime);
                    break;
                case ExpirationTimeType.Hours:
                    codeEntity.ExpirationDate = utcNow.AddHours(codeExpirationTime);
                    break;
            }
        }
    }
}
