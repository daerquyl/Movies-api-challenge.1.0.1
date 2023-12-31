﻿using System;
using System.Collections.Generic;

namespace ApiApplication.Domain.Models
{
    public class ShowtimeEntity
    {
        //TODO : make setters private
        public int Id { get; set; }
        public MovieEntity Movie { get; set; }
        public DateTime SessionDate { get; set; }
        public int AuditoriumId { get; set; }
        public ICollection<TicketEntity> Tickets { get; set; }

        public static ShowtimeEntity CreateShowtime(MovieEntity movie, int auditoriumId, DateTime sessionDate) => new ShowtimeEntity()
        {
            Movie = movie,
            AuditoriumId = auditoriumId,
            SessionDate = sessionDate,
        };
    }
}
