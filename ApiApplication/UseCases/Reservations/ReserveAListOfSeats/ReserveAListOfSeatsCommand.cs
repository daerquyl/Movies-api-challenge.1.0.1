using ApiApplication.Domain.Models;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiApplication.UseCases.Reservations.ReserveAListOfSeats
{
    public class ReserveAListOfSeats : IRequest<TicketEntity>
    {
        public int ShowtimeId { get; set; }
        [MinLength(1)]
        public List<SeatEntity> Seats { get; set; }
        public bool AllPlacesAreRequired { get; set; } = false;
    }
}
