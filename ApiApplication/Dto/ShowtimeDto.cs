using ApiApplication.Domain.Models;
using System;

namespace ApiApplication.Dto
{
    public class ShowtimeDto
    {
        public int Id { get; set; }
        public int AuditoriumId { get; set; }
        public string Movie { get; set; }

        public static ShowtimeDto FromEntity(ShowtimeEntity entity)
        {
            return new ShowtimeDto
            {
                Id = entity.Id,
                AuditoriumId = entity.AuditoriumId,
                Movie = entity.Movie.Title
            };
        }
    }
}
