using ApiApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Database.Repositories.Abstractions
{
    public interface IShowtimesRepository
    {
        Task<ShowtimeEntity> CreateShowtime(ShowtimeEntity showtimeEntity, CancellationToken cancel);
        Task<IEnumerable<ShowtimeEntity>> GetAllAsync(Expression<Func<ShowtimeEntity, bool>> filter, CancellationToken cancel);
        Task<ShowtimeEntity> GetWithMovieByIdAsync(int id, CancellationToken cancel);
        Task<ShowtimeEntity> GetWithTicketsByIdAsync(int id, CancellationToken cancel);
        Task<ShowtimeEntity> GetWithMovieAndTicketsByIdAsync(int id, CancellationToken cancel);
    }
}