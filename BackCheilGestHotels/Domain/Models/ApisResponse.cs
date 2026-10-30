namespace BackCheilGestHotels.Domain.Models
{
    public class ApisResponse<T>
    {
        public int CodeError { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        public ApisResponse( int codeError, string message,  T? data)
        {
            CodeError = codeError;
            Message = message;
            Data = data;
        }
    }
}
