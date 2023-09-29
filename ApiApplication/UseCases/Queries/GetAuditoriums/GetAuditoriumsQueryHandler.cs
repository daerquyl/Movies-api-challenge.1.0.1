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
    public class GetAuditoriumsQueryHandler : IRequestHandler<GetAuditoriumsQuery, List<Auditorium>>
    {
        private readonly IAuditoriumsRepository repository;

        public GetAuditoriumsQueryHandler(IAuditoriumsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<Auditorium>> Handle(GetAuditoriumsQuery request, CancellationToken cancel)
        {
            var auditoriums = await repository.GetAllAsync(cancel);

            return auditoriums?.Select(a => new Auditorium()
            {
                Id = a.Id.ToString(),
                NumberOfSeats = a.Seats.Count,
                Seats = a.Seats.Select(s => new Seat()
                {
                    Description = $"A{a.Id}-R{s.Row}-N{s.SeatNumber}"
                }).ToList() ?? new List<Seat>()
            }).ToList() ?? new List<Auditorium>();
        }
    }
}
