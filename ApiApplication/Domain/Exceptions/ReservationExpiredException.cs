
namespace ApiApplication.Domain.Exceptions
{
    public class ReservationExpiredException : DomainException
    {
        public ReservationExpiredException(string ticketId) : base("Domain.Reservation.ConfirmationFailed", 400, $"Confirming Reservation ({ticketId}) failed. Reservation is expired")
        {
        }
    }

    public class ReservationAlreadyConfirmedException : DomainException
    {
        public ReservationAlreadyConfirmedException(string ticketId) : base("Domain.Reservation.AlreadyConfirmed", 400, $"Confirming Reservation ({ticketId}) failed. Reservation is already confirmed")
        {
        }
    }
}