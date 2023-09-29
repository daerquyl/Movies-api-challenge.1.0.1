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
    public class GetShowtimeWithSeatsQueryHandler : IRequestHandler<GetShowtimeWithSeatsQuery, ShowtimeWithSeats>
    {
        private readonly IShowtimesRepository repository;
        private readonly IAuditoriumsRepository auditoriumRepository;

        public GetShowtimeWithSeatsQueryHandler(IShowtimesRepository repository,
            IAuditoriumsRepository auditoriumRepository)
        {
            this.repository = repository;
            this.auditoriumRepository = auditoriumRepository;
        }

        public async Task<ShowtimeWithSeats> Handle(GetShowtimeWithSeatsQuery request, CancellationToken cancel)
        {
            var showtime = await repository.GetWithMovieByIdAsync(request.ShowtimeId, cancel);
            if(showtime is null)
            {
                throw new EntityNotFoundException(nameof(showtime), request.ShowtimeId.ToString());
            }

            var auditorium = await auditoriumRepository.GetAsync(showtime.AuditoriumId, cancel);

            return new ShowtimeWithSeats()
            {
                ShowtimeId = showtime.Id,
                Movie = showtime.Movie.Title,
                Seats = auditorium.Seats?.Select(s => new Seat()
                {
                    Description = $"A{auditorium.Id}-R{s.Row}-N{s.SeatNumber}"
                }).ToList() ?? new List<Seat>()
            };
        }
    }
}
