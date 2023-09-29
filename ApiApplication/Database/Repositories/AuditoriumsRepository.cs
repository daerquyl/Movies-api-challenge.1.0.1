using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using ApiApplication.Domain.Models;
using System.Linq;
using ApiApplication.Database.Repositories.Abstractions;
using System.Collections.Generic;

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
            var auditorium = await _context.Auditoriums
                .Where(x => x.Id == auditoriumId)
                .Select(x => new
                {
                    Auditorium = x,
                    Reservations = x.Reservations.Where(r => r.ShowtimeId == showtimeId),
                    Showtimes = x.Showtimes.Where(s => s.Id == showtimeId)
                        .Select(s => new { Showtime = s, Movie = s.Movie }),
                    Seats = x.Seats
                })
                .FirstOrDefaultAsync(cancel);

            if (auditorium != null)
            {
                var result = auditorium.Auditorium;
                result.Reservations = auditorium.Reservations.ToList();
                result.Showtimes = auditorium.Showtimes.Select(s => s.Showtime).ToList();
                result.Seats = auditorium.Seats.ToList();

                return result;
            }

           return null;
        }

        public async Task<List<AuditoriumEntity>> GetAllAsync(CancellationToken cancel)
        {
            return await _context.Auditoriums
                .Include(x => x.Seats)
                .ToListAsync(cancel);
        }
    }
}
