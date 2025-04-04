
namespace Talapat.Errors
{
    public class ApiResponse
    {
        //BadRequest , NotFount
        public ApiResponse(int StatusCode, string? Message = null)
        {
            this._StatusCode = StatusCode;
            this._Message = Message ?? GetDefaultMessageForStatusCode(StatusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => "Authorized, you are not",
                404 => "Resource found, it was not",
                500 => "Errors are the path to the dark side. Errors lead to anger. Anger leads to hate. Hate leads to career change",
                _ => null
            };
        }

        public int _StatusCode { get; }
        public string? _Message { get; }
    }
}
