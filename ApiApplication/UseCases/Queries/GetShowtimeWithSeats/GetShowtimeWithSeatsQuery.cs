using ApiApplication.Domain.ReadModels;
using MediatR;

namespace ApiApplication.UseCases.Queries.GetSeatsInAuditorium
{
    public class GetShowtimeWithSeatsQuery : IRequest<ShowtimeWithSeats>
    {
        public int ShowtimeId;

        public GetShowtimeWithSeatsQuery(int showtimeId)
        {
            ShowtimeId = showtimeId;
        }
    }
}
