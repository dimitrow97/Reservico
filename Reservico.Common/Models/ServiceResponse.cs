namespace Reservico.Common.Models
{
    public class ServiceResponse
    {
        protected ServiceResponse()
        {
        }

        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }

        public static ServiceResponse Success()
        {
            return new ServiceResponse
            {
                IsSuccess = true
            };
        }

        public static ServiceResponse Error(string message)
        {
            return new ServiceResponse
            {
                ErrorMessage = message
            };
        }
    }

    public class ServiceResponse<T> : ServiceResponse
    {
        public T Data { get; set; }

        public static ServiceResponse<T> Success(T data)
        {
            return new ServiceResponse<T>
            {
                IsSuccess = true,
                Data = data
            };
        }

        public new static ServiceResponse<T> Error(string message)
        {
            return new ServiceResponse<T>
            {
                ErrorMessage = message
            };
        }
    }
}