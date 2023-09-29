namespace ApiApplication.Domain.Exceptions
{
    public class NoSeatAvailableException : DomainException
    {
        public NoSeatAvailableException(int showtimeId) : base("Domain.Reservation.NoSeatAvailable", 404, $"Reservation Failed for showtime({showtimeId}). Not seat is available")
        {
        }
    }
}