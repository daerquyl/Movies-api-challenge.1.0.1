using ApiApplication.Database.Entities;
using ApiApplication.Services;
using System;
using System.Threading.Tasks;

namespace ApiApplication.Model.Factories
{
    public class ShowtimeFactory : IShowTimeFactory
    {
        private readonly IMovieProvider movieProvider;

        public ShowtimeFactory(IMovieProvider movieProvider)
        {
            this.movieProvider = movieProvider;
        }

        public async Task<ShowtimeEntity> CreateShowtime(string movieId, int auditoriumId, DateTime sessionDate)
        {
            var movie = await movieProvider.GetMovie(movieId);
            return ShowtimeEntity.CreateShowtime(movie, auditoriumId, sessionDate);
        }
    }
}
