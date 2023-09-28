using ApiApplication.Domain.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ApiApplication.UseCases.Reservations.ReserveARangeOfSeats
{
    public class ReserveARangeOfSeats: IRequest<TicketEntity>
    {
        public int ShowtimeId { get; set; }
        [MinLength(1)]
        public int numberOfSeats { get; set; }
        public bool AllPlacesAreRequired { get; set; } = false;
    }
}
