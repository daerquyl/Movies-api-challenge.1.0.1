using ApiApplication.Domain.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ApiApplication.Domain.services
{
    public partial class ReservationService : IReservationService
    {
        public TicketEntity ReserveListOfSeats(AuditoriumEntity auditorium, int showtimeId, List<SeatEntity> seats, DateTime reservationDate, bool allPlacesRequired = false)
        {
            ThrowExceptionIfAllSeatsAreNotContiguous(seats, showtimeId);

            var alreadyReservedSeats = auditorium.GetFreeSeats(showtimeId);

            var availableSeats = GetAvailablesSeats(alreadyReservedSeats.ToList(), showtimeId, seats, allPlacesRequired);
            var reservation = TicketEntity.Create(showtimeId, availableSeats, reservationDate);

            auditorium.AddReservation(reservation);

            return reservation;
        }


        public TicketEntity ReserveRangeSeats(AuditoriumEntity auditorium, int showtimeId, int numberOfPlaces, DateTime reservationDate, bool allPlacesRequired = false)
        {

            var alreadyReservedSeats = auditorium.GetFreeSeats(showtimeId);

            var seats = GetAvailablesSeats(auditorium.Seats.ToList(), alreadyReservedSeats.ToList(), showtimeId, numberOfPlaces, allPlacesRequired);
            var reservation = TicketEntity.Create(showtimeId, seats, reservationDate);

            auditorium.AddReservation(reservation);

            return reservation;
        }

        public void ConfirmReservation(AuditoriumEntity auditorium, Guid ticketId)
        {
            auditorium.ConfirmPayment(ticketId);
        }
    }
}

