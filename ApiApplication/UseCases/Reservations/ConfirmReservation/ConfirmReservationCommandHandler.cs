using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Domain.Models;
using ApiApplication.Domain.services;
using ApiApplication.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.UseCases.Reservations.ConfirmReservation
{
    public class ConfirmReservationCommandHandler : IRequestHandler<ConfirmReservation>
    {
        private readonly IShowtimesRepository showtimeRepository;
        private readonly ITicketsRepository ticketsRepository;
        private readonly IAuditoriumsRepository auditoriumsRepository;
        private readonly IReservationService reservationService;

        public ConfirmReservationCommandHandler(
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

        public async Task<Unit> Handle(ConfirmReservation command, CancellationToken cancel)
        {
            var reservation = await GetReservationIfExistsOrThrowException(command, cancel);
            var showtime = await showtimeRepository.GetWithMovieByIdAsync(reservation.ShowtimeId, cancel);
            var auditorum = await auditoriumsRepository.GetAsync(showtime.AuditoriumId, showtime.Id, cancel);

            reservationService.ConfirmReservation(auditorum, reservation.Id);

            try
            {
                await ticketsRepository.CreateAsync(reservation, cancel);
                return Unit.Value;
            }
            catch (Exception e)
            {
                throw new SqlException(e.Message);
            }
        }

        private async Task<TicketEntity> GetReservationIfExistsOrThrowException(ConfirmReservation command, CancellationToken cancel)
        {
            var reservation = await ticketsRepository.GetAsync(command.ReservationId, cancel);
            if(reservation is null)
            {
                throw new UserInputValidationException("InvalidOperation.ConfirmReservation.UnknowShowtime", $"Cannot find a reservation with Id {command.ReservationId}");
            }

            return reservation;
        }
    }
}
