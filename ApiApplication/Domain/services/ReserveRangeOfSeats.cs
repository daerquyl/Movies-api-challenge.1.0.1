using ApiApplication.Domain.Exceptions;
using System.Collections.Generic;
using System;
using System.Linq;
using ApiApplication.Domain.Models;

namespace ApiApplication.Domain.services
{
    public partial class ReservationService
    {
        private List<SeatEntity> GetAvailablesSeats(List<SeatEntity> auditoriumSeats, List<SeatEntity> reservedSeats, int showtime, int numberOfPlaces, bool allPlacesRequired = false)
        {
            var availableSeats = new List<SeatEntity>();

            TryGetEnoughSeats(numberOfPlaces, reservedSeats, availableSeats);

            return availableSeats;

            void TryGetEnoughSeats(int numberOfPlaces, List<SeatEntity> reservedSeats, List<SeatEntity> availableSeats)
            {
                int largestAvailableStartIndex, largestAvailableEndIndex;
                Predicate<SeatEntity> isNotAlreadyReserved = seat => !reservedSeats.Any(reservedSeat => reservedSeat == seat);
                GetLargestContiguousSeats(numberOfPlaces, auditoriumSeats, isNotAlreadyReserved, out largestAvailableStartIndex, out largestAvailableEndIndex);

                for (var j = largestAvailableStartIndex; j < largestAvailableEndIndex; j++)
                {
                    availableSeats.Add(auditoriumSeats.ElementAt(j));
                }

                if (allPlacesRequired && availableSeats.Count < numberOfPlaces)
                {
                    throw new SeatAlreadyReservedException(showtime);
                }
            }
        }

        private void GetLargestContiguousSeats(int numberOfPlaces, ICollection<SeatEntity> allSeats, Predicate<SeatEntity> seatIsValid, out int largestAvailableStartIndex, out int largestAvailableEndIndex)
        {
            var currentRangeIndex = 0;
            SeatEntity seat = allSeats.ElementAt(0);
            SeatEntity previousSeat = default;

            var ranges = new Dictionary<int, int>() { { 0, 0 } };

            var i = 0;
            while (i < allSeats.Count)
            {
                previousSeat = seat;
                seat = allSeats.ElementAt(i);
                if (seatIsValid(seat))
                {
                    ranges[currentRangeIndex]++;
                }
                else if (ranges[currentRangeIndex] < numberOfPlaces)
                {
                    currentRangeIndex = i + 1;
                    ranges.Add(currentRangeIndex, 0);
                }
                i++;
            }

            largestAvailableStartIndex = ranges.OrderByDescending(r => r.Value).First().Key;
            largestAvailableEndIndex = largestAvailableStartIndex + Math.Min(ranges.OrderByDescending(r => r.Value).First().Value, numberOfPlaces);
        }
    }
}

