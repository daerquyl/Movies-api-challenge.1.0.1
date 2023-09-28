namespace ApiApplication.Domain.Exceptions
{
    public class SeatAlreadyBoughtException : DomainException
    {
        public SeatAlreadyBoughtException(int showtimeId) : base("Domain.Reservation.SeatAlreadyBought", 400, $"Reservation Failed for showtime({showtimeId}). Not all seats are available.")
        {
        }

        public SeatAlreadyBoughtException(string ticketID) : base("Domain.Reservation.SeatAlreadyBought", 400, $"Confirming Reservation showtime({ticketID}) failed. Some seats are already bought.")
        {
        }
    }
}