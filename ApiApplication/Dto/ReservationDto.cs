using ApiApplication.Domain.Models;
using System;

namespace ApiApplication.Dto
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public int AuditoriumId { get; set; }
        public int NumberOfSeats { get; set; }
        public string Movie { get; set; }

        public static ReservationDto FromEntity(TicketEntity entity)
        {
            return new ReservationDto
            {
                Id = entity.Id,
                AuditoriumId = entity.Showtime.AuditoriumId,
                NumberOfSeats = entity.Seats.Count,
                Movie = entity.Showtime.Movie.Title
            };
        }
    }
}
