using System;

namespace ApiApplication.Domain.ReadModels
{
    public class Showtime
    {
        public string Id { get; set; }
        public string Movie { get; set; }
        public DateTime SessionDate { get; set; }
        public int AuditoriumId { get; internal set; }
    }
}