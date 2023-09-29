using ApiApplication.Domain.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ApiApplication.UseCases.Commands.Reservations.ReserveARangeOfSeats
{
    public class ReserveARangeOfSeatsCommand : IRequest<TicketEntity>
    {
        public int ShowtimeId { get; set; }
        [Range(1, int.MaxValue)]
        public int numberOfSeats { get; set; }
        public bool AllPlacesAreRequired { get; set; } = false;
    }
}
