namespace Application.Common
{
    public class RequestResult<T>
    {
        public int StatusCode { get; set; }
        public bool Success { get; private set; }
        public string Message { get; set; }
        public IEnumerable<T> Data { get; private set; }
        public string Details { get; set; }

        public RequestResult(bool success, string message, IEnumerable<T> data, int statusCode = 200, string details = null)
        {
            StatusCode = statusCode;
            Success = success;
            Message = message;
            Data = data;
            Details = details;
        }

        public static RequestResult<T> SuccessResult(IEnumerable<T> data)
        {
            return new RequestResult<T>(true, null, data);
        }

        public static RequestResult<T> FailureResult(string message, string details, int statusCode = 400)
        {
            return new RequestResult<T>(false, message, default, statusCode, details);
        }
    }
}
