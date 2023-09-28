using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Domain.Models;
using ApiApplication.Domain.services;
using ApiApplication.Exceptions;
using ApiApplication.UseCases.Reservations.ReserveAListOfSeats;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.UseCases.Reservations.ReserveAListOfSeats
{
    public class ReserveAListOfSeatsCommandHandler : IRequestHandler<ReserveAListOfSeats, TicketEntity>
    {
        private readonly IShowtimesRepository showtimeRepository;
        private readonly ITicketsRepository ticketsRepository;
        private readonly IAuditoriumsRepository auditoriumsRepository;
        private readonly IReservationService reservationService;

        public ReserveAListOfSeatsCommandHandler(
            IShowtimesRepository showtimeRepository,
            ITicketsRepository ticketsRepository,
            IAuditoriumsRepository auditoriumsRepository,
            IReservationService reservationService)
        {
            this.showtimeRepository = showtimeRepository;
            this.ticketsRepository = ticketsRepository;
            this.auditoriumsRepository = auditoriumsRepository;
            this.reservationService = reservationService;
        }

        public async Task<TicketEntity> Handle(ReserveAListOfSeats command, CancellationToken cancel)
        {
            var showtime = await GetShowtimeIfExistsOrThrowException(command, cancel);
            var auditorum = await auditoriumsRepository.GetAsync(showtime.AuditoriumId, showtime.Id, cancel);

            var reservation = reservationService.ReserveListOfSeats(auditorum, command.ShowtimeId, command.Seats, DateTime.Now, command.AllPlacesAreRequired);

            try
            {
                var saved = await ticketsRepository.CreateAsync(reservation, cancel);
                return saved;
            }
            catch (Exception e)
            {
                throw new SqlException(e.Message);
            }
        }

        private async Task<ShowtimeEntity> GetShowtimeIfExistsOrThrowException(ReserveAListOfSeats command, CancellationToken cancel)
        {
            var showtime = await showtimeRepository.GetWithMovieByIdAsync(command.ShowtimeId, cancel);
            if(showtime is null)
            {
                throw new UserInputValidationException("InvalidOperation.ReserveSeats.UnknowShowtime", "Cannot create reservation without in valid showtime");
            }

            return showtime;
        }
    }
}
