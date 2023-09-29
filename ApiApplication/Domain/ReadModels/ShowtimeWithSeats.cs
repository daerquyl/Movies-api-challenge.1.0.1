using System.Collections.Generic;

namespace ApiApplication.Domain.ReadModels
{
    public class ShowtimeWithSeats
    {
        public int ShowtimeId { get; set; }
        public string Movie { get; set; }
        public List<Seat> Seats { get; set; }
    }
}
