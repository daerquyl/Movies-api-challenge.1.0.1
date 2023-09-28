using ApiApplication.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Database.Repositories.Abstractions
{
    public interface IAuditoriumsRepository
    {
        Task<AuditoriumEntity> GetAsync(int auditoriumId, CancellationToken cancel);
        Task<AuditoriumEntity> GetAsync(int auditoriumId, int showtimeId, CancellationToken cancel);
    }
}