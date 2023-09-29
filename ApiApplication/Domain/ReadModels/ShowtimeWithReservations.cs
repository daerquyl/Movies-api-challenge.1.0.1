using ApiApplication.Domain.Models;
using System.Collections.Generic;

namespace ApiApplication.Domain.ReadModels
{
    public class ShowtimeWithReservations
    {
        public int ShowtimeId { get; set; }
        public string Movie { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}
