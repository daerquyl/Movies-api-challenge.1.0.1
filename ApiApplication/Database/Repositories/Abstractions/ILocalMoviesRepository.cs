using ApiApplication.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Database.Repositories.Abstractions
{
    public interface ILocalMovieRepository
    {
        Task<MovieEntity> GetByImdbIdAsync(string imdbId, CancellationToken cancel);
    }
}