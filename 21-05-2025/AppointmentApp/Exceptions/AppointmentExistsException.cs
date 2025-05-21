namespace AppointmentApp.Exceptions
{
    public class AppointmentExistsException : Exception
    {
        public AppointmentExistsException()
        {
        }

        public AppointmentExistsException(string? message) : base(message)
        {

        }
    }
}