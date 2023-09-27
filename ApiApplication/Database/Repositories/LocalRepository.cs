using ApiApplication.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
using System.Linq.Expressions;
using ApiApplication.Database.Repositories.Abstractions;

namespace ApiApplication.Database.Repositories
{
    public class LocalMovieRepository : ILocalMovieRepository
    {
        private readonly CinemaContext _context;

        public LocalMovieRepository(CinemaContext context)
        {
            _context = context;
        }

        public async Task<MovieEntity> GetByImdbIdAsync(string id, CancellationToken cancel)
        {
            return await _context.Movies
                .FirstOrDefaultAsync(x => x.ImdbId == id, cancel);
        }
    }
}
