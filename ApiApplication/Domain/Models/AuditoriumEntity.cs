using ApiApplication.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiApplication.Domain.Models
{
    public class AuditoriumEntity
    {
        //TODO : make setters private
        //Maybe access reservations through showtime
        //Maybe doing simple crud+ anemic model

        public int Id { get; set; }
        public List<ShowtimeEntity> Showtimes { get; set; } = new List<ShowtimeEntity>();
        public ICollection<SeatEntity> Seats { get; set; }
        public List<TicketEntity> Reservations { get; set; } = new List<TicketEntity>();


        public void ScheduleShowtime(ShowtimeEntity showtime)
        {
            if(showtime is null) 
            {
                throw new InvalidOperationException($"Trying to schedule empty showtime in auditorium ({Id})");
            }

            Showtimes.Add(showtime);
        }

        public void AddReservation(TicketEntity reservation)
        {
            if (reservation is null)
            {
                throw new InvalidOperationException($"Invalid reservation details)");
            }

            Reservations.Add(reservation);
        }

        internal void ConfirmPayment(Guid reservationId)
        {
            var reservation = Reservations.First(r => r.Id == reservationId);

            if (reservation is null)
            {
                throw new InvalidOperationException($"Invalid reservation details)");
            }

            if (reservation.IsSold)
            {
                throw new ReservationAlreadyConfirmedException(reservationId.ToString());
            }

            var soldSeats = GetSoldSeats(reservation.ShowtimeId);

            if(reservation.Seats.Any(s => soldSeats.Contains(s)))
            {
                throw new SeatAlreadyBoughtException(reservationId.ToString());
            }

            reservation.ConfirmPayment();
        }

        public IReadOnlyCollection<SeatEntity> GetFreeSeats(int showtime)
        {
            return Reservations?
                .Where(r => r.ShowtimeId == showtime && !r.IsExpired && !r.IsSold)
                .SelectMany(r => r.Seats)
                .ToList();
        }

        public IReadOnlyCollection<SeatEntity> GetSoldSeats(int showtime)
        {
            return Reservations?
                .Where(r => r.ShowtimeId == showtime && r.IsSold)
                .SelectMany(r => r.Seats)
                .ToList();
        }
    }
}
