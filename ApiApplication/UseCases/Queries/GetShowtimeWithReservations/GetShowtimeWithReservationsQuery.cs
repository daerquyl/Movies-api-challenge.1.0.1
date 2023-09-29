using ApiApplication.Domain.ReadModels;
using MediatR;

namespace ApiApplication.UseCases.Queries.GetSeatsInAuditorium
{
    public class GetShowtimeWithReservationsQuery : IRequest<ShowtimeWithReservations>
    {
        public int ShowtimeId;

        public GetShowtimeWithReservationsQuery(int showtimeId)
        {
            ShowtimeId = showtimeId;
        }
    }
}
