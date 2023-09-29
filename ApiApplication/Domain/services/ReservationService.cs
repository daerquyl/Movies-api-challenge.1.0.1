using ApiApplication.Domain.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using ApiApplication.Domain.Exceptions;

namespace ApiApplication.Domain.services
{
    public partial class ReservationService : IReservationService
    {
        public TicketEntity ReserveListOfSeats(AuditoriumEntity auditorium, int showtimeId, List<SeatEntity> seats, DateTime reservationDate, bool allPlacesRequired = false)
        {
            ThrowExceptionIfAllSeatsAreNotContiguous(seats, showtimeId);

            var reservedSeats = auditorium.GetReservedSeats(showtimeId);
            var availableSeats = GetAvailablesSeats(reservedSeats.ToList(), showtimeId, seats, allPlacesRequired);

            return ReserveSeats(auditorium, showtimeId, reservationDate, availableSeats);
        }

        public TicketEntity ReserveRangeSeats(AuditoriumEntity auditorium, int showtimeId, int numberOfPlaces, DateTime reservationDate, bool allPlacesRequired = false)
        {

            var reservedSeats = auditorium.GetReservedSeats(showtimeId);

            var availableSeats = GetAvailablesSeats(auditorium.Seats.ToList(), reservedSeats.ToList(), showtimeId, numberOfPlaces, allPlacesRequired);

            return ReserveSeats(auditorium, showtimeId, reservationDate, availableSeats);
        }

        private static TicketEntity ReserveSeats(AuditoriumEntity auditorium, int showtimeId, DateTime reservationDate, List<SeatEntity> availableSeats)
        {
            if (availableSeats.Count == 0)
            {
                throw new NoSeatAvailableException(showtimeId);
            }

            var reservation = TicketEntity.Create(showtimeId, availableSeats, reservationDate);

            auditorium.AddReservation(reservation);

            return reservation;
        }

        public void ConfirmReservation(AuditoriumEntity auditorium, Guid ticketId)
        {
            auditorium.ConfirmPayment(ticketId);
        }
    }
}

