using ApiApplication.Domain.ReadModels;
using MediatR;
using System.Collections.Generic;

namespace ApiApplication.UseCases.Queries.GetSeatsInAuditorium
{
    public class GetShowtimesQuery : IRequest<List<Showtime>>
    {
    }
}
