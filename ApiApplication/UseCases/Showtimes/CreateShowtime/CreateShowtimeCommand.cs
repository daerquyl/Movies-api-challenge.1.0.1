using MediatR;
using System;

namespace ApiApplication.UseCases.Showtimes.CreateShowtime
{
    public class CreateShowtimeCommand: IRequest<int>
    {
        public int AuditoriumId { get; set; }
        public string MovieId { get; set; }
        public DateTime SessionDate { get; set; }
    }
}
