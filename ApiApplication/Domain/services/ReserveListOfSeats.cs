using ApiApplication.Domain.Models;
using ApiApplication.Domain.Exceptions;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ApiApplication.Domain.services
{
    public partial class ReservationService
    {
        private List<SeatEntity> GetAvailablesSeats(List<SeatEntity> alreadyReservedSeats, int showtime, List<SeatEntity> seats, bool allPlacesRequired = false)
        {
            var availableSeats = new List<SeatEntity>();

            TryGetEnoughSeats(showtime, seats, allPlacesRequired, alreadyReservedSeats, availableSeats);

            return availableSeats;

            void TryGetEnoughSeats(int showtime, List<SeatEntity> seats, bool allPlacesRequired, List<SeatEntity> alreadyReservedSeats, List<SeatEntity> availableSeats)
            {
                foreach (var seat in seats)
                {
                    if (!alreadyReservedSeats.Any(reservedSeat => reservedSeat == seat))
                    {
                        if (allPlacesRequired)
                        {
                            throw new SeatAlreadyReservedException(showtime);
                        }
                        availableSeats.Add(seat);
                    }
                }
            }
        }

        private void ThrowExceptionIfAllSeatsAreNotContiguous(List<SeatEntity> seats, int showtimeId)
        {
            if (seats.Count <= 1) return;

            for(var i=0; i<seats.Count; i++)
            {
                var isContiguous = false;
                for (var j=i+1; j<seats.Count; j++)
                {
                    if (IsContiguousTo(seats[i], seats[j]))
                    {
                        isContiguous = true;
                        break;
                    }
                }
                if(!isContiguous) {
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

