namespace ApiApplication.Domain.Exceptions
{
    public class SeatAlreadyReservedException : DomainException
    {
        public SeatAlreadyReservedException(int showtimeId) : base("Domain.Reservation.SeatAlreadyReserved", 400, $"Reservation Failed for showtime({showtimeId}). Not all seats are available")
        {
        }
    }
}