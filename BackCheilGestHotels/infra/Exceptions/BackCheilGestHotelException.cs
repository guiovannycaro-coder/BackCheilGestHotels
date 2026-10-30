namespace BackCheilGestHotels.infra.Exceptions
{
    public class BackCheilGestHotelException : Exception
    {
        public int StatusCode { get; }

        public BackCheilGestHotelException(int statusCode,string message,Exception? cause)
           : base(message, cause)
        {
            StatusCode = statusCode;
        }
    } 
}
