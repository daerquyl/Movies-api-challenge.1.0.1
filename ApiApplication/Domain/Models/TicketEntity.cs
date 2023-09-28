using ApiApplication.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace ApiApplication.Domain.Models
{
    public class TicketEntity
    {
        public TicketEntity()
        {
            CreatedTime = DateTime.Now;
            Paid = false;
        }

        public const int RESERVATION_DURATION = 10;

        //TODO : make setters private
        public Guid Id { get; set; }
        public int ShowtimeId { get; set; }
        public ICollection<SeatEntity> Seats { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool Paid { get; set; }
        public ShowtimeEntity Showtime { get; set; }


        internal static TicketEntity Create(int showtimeId, List<SeatEntity> seats, DateTime reserationDate) => new TicketEntity()
        {
            Id = Guid.NewGuid(),
            ShowtimeId = showtimeId,
            Seats = seats,
            CreatedTime = reserationDate
        };

        internal void ConfirmPayment()
        {
            Paid = true;
        }

        internal bool IsExpired => DateTime.Now.AddMinutes(-RESERVATION_DURATION) > CreatedTime;
        internal bool IsSold => Paid;
    }
}
