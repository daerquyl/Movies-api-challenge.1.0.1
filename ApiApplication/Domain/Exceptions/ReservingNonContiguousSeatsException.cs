namespace ApiApplication.Domain.Exceptions
{
    public class ReservingNonContiguousSeatsException : DomainException
    {
        public ReservingNonContiguousSeatsException(int showtimeId) : base("Domain.Reservation.NonContiguousSeats", 400, $"Reservation for showtime ({showtimeId}) Failed. Not all seats are contiguous")
        {
        }
    }
}