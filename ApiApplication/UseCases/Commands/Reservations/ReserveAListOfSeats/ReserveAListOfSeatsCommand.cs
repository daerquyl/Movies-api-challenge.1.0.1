using ApiApplication.Domain.Models;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiApplication.UseCases.Commands.Reservations.ReserveAListOfSeats
{
    public class ReserveAListOfSeatsCommand : IRequest<TicketEntity>
    {
        public int ShowtimeId { get; set; }
        [MinLength(1)]
        public List<LightSeat> Seats { get; set; }
        public bool AllPlacesAreRequired { get; set; }
    }

    public class LightSeat{
        public short Row { get; set; }
        public short SeatNumber { get; set; }
    }
}
