using System;
using System.Security.Policy;

namespace ApiApplication.Domain.Models
{
    public class SeatEntity
    {
        //TODO : make setters private
        public short Row { get; set; }
        public short SeatNumber { get; set; }
        public int AuditoriumId { get; set; }
        public DateTime? ReservedSince { get; set; } = null;
        public AuditoriumEntity Auditorium { get; set; }
    }
}
