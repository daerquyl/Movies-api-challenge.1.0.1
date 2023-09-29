using ApiApplication.Domain.Models;
using ApiApplication.Domain.Exceptions;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ApiApplication.Domain.services
{
    public partial class ReservationService
    {
        private List<SeatEntity> GetAvailablesSeats(List<SeatEntity> reservedSeats, int showtime, List<SeatEntity> seats, bool allPlacesRequired = false)
        {
            var availableSeats = new List<SeatEntity>();

            TryGetEnoughSeats(showtime, seats, allPlacesRequired, reservedSeats, availableSeats);

            return availableSeats;

            void TryGetEnoughSeats(int showtime, List<SeatEntity> seats, bool allPlacesRequired, List<SeatEntity> reservedSeats, List<SeatEntity> availableSeats)
            {
                foreach (var seat in seats)
                {
                    if (!reservedSeats.Any(reservedSeat => reservedSeat == seat))
                    {
                        availableSeats.Add(seat);
                    }else if (allPlacesRequired)
                    {
                        throw new SeatAlreadyReservedException(showtime);
                    }
                }
            }
        }

        private void ThrowExceptionIfAllSeatsAreNotContiguous(List<SeatEntity> seats, int showtimeId)
        {
            if (seats.Count <= 1) return;

            foreach (var seat in seats)
            {
                var isContiguousToAnySeat = false;

                foreach (var otherSeat in seats)
                {
                    if (seat != otherSeat && IsContiguousTo(seat, otherSeat))
                    {
                        isContiguousToAnySeat = true;
                        break;
                    }
                }

                if (!isContiguousToAnySeat)
                {
                    throw new ReservingNonContiguousSeatsException(showtimeId);
                }
            }
        }

        private bool IsContiguousTo(SeatEntity seatA, SeatEntity seatB)
        {
            var isAdjacent = Math.Abs(seatA.SeatNumber - seatB.SeatNumber) == 1 && seatA.Row == seatB.Row;
            var isFrontOrBehind = Math.Abs(seatA.Row - seatB.Row) == 1 && seatA.SeatNumber == seatB.SeatNumber;
            return isAdjacent || isFrontOrBehind;
        }
    }
}

