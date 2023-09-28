using System;
using System.Collections.Generic;

namespace ApiApplication.Domain.Models
{
    public class MovieEntity
    {
        //TODO : make setters private
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImdbId { get; set; }
        public string Stars { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<ShowtimeEntity> Showtimes { get; set; }

        internal static MovieEntity Create(string title, DateTime releaseDate, string stars, string imdbId) => new MovieEntity()
        {
            Title = title,
            ImdbId = imdbId,
            Stars = stars,
            ReleaseDate = releaseDate,
        };
    }
}
