using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Domain.Exceptions;
using ApiApplication.Domain.Models;
using ApiApplication.Domain.services;
using ApiApplication.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.UseCases.Commands.Reservations.ReserveAListOfSeats
{
    public class ReserveAListOfSeatsCommandHandler : IRequestHandler<ReserveAListOfSeatsCommand, TicketEntity>
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

        //Should add a lock here 
        public async Task<TicketEntity> Handle(ReserveAListOfSeatsCommand command, CancellationToken cancel)
        {
            var showtime = await GetShowtimeIfExistsOrThrowException(command, cancel);
            var auditorum = await auditoriumsRepository.GetAsync(showtime.AuditoriumId, showtime.Id, cancel);
            var seats = GetAuditoriumSeats(auditorum, command.Seats.Distinct().ToList());

            var reservation = reservationService.ReserveListOfSeats(auditorum, command.ShowtimeId, seats, DateTime.Now, command.AllPlacesAreRequired);

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

        private List<SeatEntity> GetAuditoriumSeats(AuditoriumEntity auditorium, List<LightSeat> seats)
        {
            var auditoriumSeats = auditorium.Seats?
                .Where(entity => seats.Any(s => s.Row == entity.Row && s.SeatNumber == entity.SeatNumber))
                .ToList() ?? new List<SeatEntity>();

            if(auditoriumSeats.Count != seats.Count)
            {
                throw new EntityNotFoundException(nameof(SeatEntity), null);
            }

            return auditoriumSeats;
        }

        private async Task<ShowtimeEntity> GetShowtimeIfExistsOrThrowException(ReserveAListOfSeatsCommand command, CancellationToken cancel)
        {
            var showtime = await showtimeRepository.GetWithMovieByIdAsync(command.ShowtimeId, cancel);
            if (showtime is null)
            {
                throw new UserInputValidationException("InvalidOperation.ReserveSeats.UnknowShowtime", "Cannot create reservation without in valid showtime");
            }

            return showtime;
        }
    }
}
