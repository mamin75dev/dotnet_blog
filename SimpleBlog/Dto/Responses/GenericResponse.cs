namespace SimpleBlog.Dto.Responses
{
    public class GenericResponse<T>
    {
        public int StatusCode;
        public string Message;
        public T Data;

        public GenericResponse(int statusCode, string message, T data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }
    }
}
