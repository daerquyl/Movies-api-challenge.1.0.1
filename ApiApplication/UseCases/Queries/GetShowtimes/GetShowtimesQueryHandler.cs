using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Domain.Exceptions;
using ApiApplication.Domain.ReadModels;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.UseCases.Queries.GetSeatsInAuditorium
{
    public class GetShowtimesQueryHandler : IRequestHandler<GetShowtimesQuery, List<Showtime>>
    {
        private readonly IShowtimesRepository repository;

        public GetShowtimesQueryHandler(IShowtimesRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<Showtime>> Handle(GetShowtimesQuery request, CancellationToken cancel)
        {
            var showtimes = await repository.GetAllAsync(null, cancel);

            return showtimes?.Select(s => new Showtime()
            {
                Id = s.Id.ToString(),
                AuditoriumId = s.AuditoriumId,
                Movie = s.Movie.Title,
                SessionDate = s.SessionDate
            }).ToList() ?? new List<Showtime>();
        }
    }
}
