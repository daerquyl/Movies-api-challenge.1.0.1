using System.Collections.Generic;

namespace ApiApplication.Domain.ReadModels
{
    public class Auditorium
    {
        public string Id { get; set; }
        public int NumberOfSeats { get; set; }
        public List<Seat> Seats { get; set; }
    }
}