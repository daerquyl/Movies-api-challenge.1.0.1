using ApiApplication.Database.Entities;
using System.Threading.Tasks;

namespace ApiApplication.Services
{
    public interface IMovieProvider
    {
        Task<MovieEntity> GetMovie(string movieId);
    }
}