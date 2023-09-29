using ApiApplication.Domain.Models;
using MediatR;
using System;

namespace ApiApplication.UseCases.Commands.Showtimes.CreateShowtime
{
    public class CreateShowtimeCommand : IRequest<ShowtimeEntity>
    {
        public int AuditoriumId { get; set; }
        public string MovieId { get; set; }
        public DateTime SessionDate { get; set; }
    }
}
