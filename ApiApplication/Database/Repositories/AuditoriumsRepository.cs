using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Domain.Models;
using System.Linq;

namespace ApiApplication.Database.Repositories
{
    public class AuditoriumsRepository : IAuditoriumsRepository
    {
        private readonly CinemaContext _context;

        public AuditoriumsRepository(CinemaContext context)
        {
            _context = context;
        }

        public async Task<AuditoriumEntity> GetAsync(int auditoriumId, CancellationToken cancel)
        {
            return await _context.Auditoriums
                .Include(x => x.Seats)
                .FirstOrDefaultAsync(x => x.Id == auditoriumId, cancel);
        }

        public async Task<AuditoriumEntity> GetAsync(int auditoriumId, int showtimeId, CancellationToken cancel)
        {
            return await _context.Auditoriums
                .Include(x => x.Reservations.Where(r => r.ShowtimeId == showtimeId))
                .Include(x => x.Showtimes.Where(s => s.Id == showtimeId))
                    .ThenInclude(x => x.Movie)
                .Include(x => x.Seats)
                .FirstOrDefaultAsync(x => x.Id == auditoriumId, cancel);
        }
    }
}
