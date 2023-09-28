using ApiApplication.Domain.Models;
using System.Collections.Generic;
using System;

namespace ApiApplication.Domain.services
{
    public interface IReservationService
    {
        TicketEntity ReserveListOfSeats(AuditoriumEntity auditorium, int showtimeId, List<SeatEntity> seats, DateTime reservationDate, bool allPlacesRequired = false);
        TicketEntity ReserveRangeSeats(AuditoriumEntity auditorium, int showtimeId, int numberOfPlaces, DateTime reservationDate, bool allPlacesRequired = false);
        void ConfirmReservation(AuditoriumEntity auditorium, Guid ticketId);
    }
}