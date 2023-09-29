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
    public class GetShowtimeWithReservationsQueryHandler : IRequestHandler<GetShowtimeWithReservationsQuery, ShowtimeWithReservations>
    {
        private readonly IShowtimesRepository repository;

        public GetShowtimeWithReservationsQueryHandler(IShowtimesRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ShowtimeWithReservations> Handle(GetShowtimeWithReservationsQuery request, CancellationToken cancel)
        {
            var showtime = await repository.GetWithMovieAndTicketsByIdAsync(request.ShowtimeId, cancel);
            if(showtime is null)
            {
                throw new EntityNotFoundException(nameof(showtime), request.ShowtimeId.ToString());
            }

            return new ShowtimeWithReservations()
            {
                ShowtimeId = showtime.Id,
                Movie = showtime.Movie.Title,
                Reservations = showtime.Tickets?.Select(r => new Reservation
                {
                    Id = r.Id.ToString(),
                    NumberfSeats = r.Seats.Count
                }).ToList() ?? new List<Reservation>()
            };
        }
    }
}
