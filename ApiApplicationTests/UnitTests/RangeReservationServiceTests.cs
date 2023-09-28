using ApiApplication.Database;
using ApiApplication.Domain.Exceptions;
using ApiApplication.Domain.Models;
using ApiApplication.Domain.services;
using FluentAssertions;

namespace ApiApplicationTests.UnitTests
{
    public partial class ReservationServiceTests
    {
        [Fact]
        public void Shoud_ReserveSeatsForAMovie()
        {
            //Given a number of available seat in a auditorium
            var auditorium = new AuditoriumEntity { Id = 1, Seats = SampleData.GenerateSeats(1, 3, 4) };

            //And given a showtime is scheduled in this auditorium
            var showtime = new ShowtimeEntity { Id = 2, AuditoriumId = auditorium.Id };
            auditorium.ScheduleShowtime(showtime);

            //When reserving seats
            var reservationService = new ReservationService();
            var reservation = reservationService.ReserveRangeSeats(auditorium, 2, showtime.Id, DateTime.Now);

            //Then Reservation is created
            auditorium.Reservations.Should().HaveCount(1);

            reservation.Id.Should().NotBeEmpty();
            reservation.Seats.Should().AllSatisfy(seat => seat.Should().BeOfType<SeatEntity>().And.NotBeNull())
                .And
                .HaveCount(2);
            auditorium.Reservations.Single().ShowtimeId.Should().Be(showtime.Id);
        }

        [Fact]
        public void Shoud_NotReservedSameSeatTwoTimesIn10Minutes()
        {
            //Given a number of available seat in a auditorium

            var auditorium = new AuditoriumEntity { Id = 1, Seats = SampleData.GenerateSeats(1, 2, 10) };

            //And given a showtime is scheduled in this auditorium
            var showtime = new ShowtimeEntity { Id = 2, AuditoriumId = auditorium.Id };
            auditorium.ScheduleShowtime(showtime);

            //When reserving seats already reserved in less than 10 minutes
            //Reserve significant number of seats but not all
            var remainingSeats = 5;

            var reservationService = new ReservationService();
            var reservation1 = reservationService.ReserveRangeSeats(auditorium, showtime.Id, auditorium.Seats.Count - remainingSeats, DateTime.Now);

            //Reserve all seats
            var reservation2 = reservationService.ReserveRangeSeats(auditorium, showtime.Id, auditorium.Seats.Count, DateTime.Now);

            //Then the last reservation contains only seat not reserved from the first reservation
            reservation2.Seats.Should().HaveCount(5)
                .And.OnlyContain(seat => !reservation1.Seats
                    .Any(s1 => seat.SeatNumber == s1.SeatNumber && seat.Row == s1.Row))
                .And
                .HaveCount(remainingSeats);
        }

        [Fact]
        public void Shoud_BeAbleToReservedSameSeatAfter10Minutes()
        {
            //Given a number of available seat in a auditorium
            var auditorium = new AuditoriumEntity { Id = 1, Seats = SampleData.GenerateSeats(1, 2, 10) };

            //And given a showtime is scheduled in this auditorium
            var showtime = new ShowtimeEntity { Id = 2, AuditoriumId = auditorium.Id };
            auditorium.ScheduleShowtime(showtime);

            //When reserving seats already reserved more than 10 minutes ago
            //Reserve significant number of seats but not all
            var remainingSeats = 5;

            var reservationService = new ReservationService();
            reservationService.ReserveRangeSeats(auditorium, showtime.Id, auditorium.Seats.Count - remainingSeats, DateTime.Now.AddMinutes(-10));

            //Reserve all seats
            var reservation2 = reservationService.ReserveRangeSeats(auditorium, showtime.Id, auditorium.Seats.Count, DateTime.Now);

            //Then the last reservation contains seats reserved from the first reservation
            reservation2.Seats.Should().HaveCount(auditorium.Seats.Count);
        }


        [Fact]
        public void Shoud_BeAbleToReservedSameSeatForDifferentShowtimes()
        {
            //Given a number of available seat in a auditorium
            var auditorium = new AuditoriumEntity { Id = 1, Seats = SampleData.GenerateSeats(1, 2, 10) };

            //And given a showtime is scheduled in this auditorium
            var showtime1 = new ShowtimeEntity { Id = 2, AuditoriumId = auditorium.Id };
            var showtime2 = new ShowtimeEntity { Id = 3, AuditoriumId = auditorium.Id };
            auditorium.ScheduleShowtime(showtime1);
            auditorium.ScheduleShowtime(showtime2);

            //When reserving seats already reserved more than 10 minutes ago
            //Reserve significant number of seats but not all
            var remainingSeats = 5;

            var reservationService = new ReservationService();
            reservationService.ReserveRangeSeats(auditorium, showtime1.Id, auditorium.Seats.Count - remainingSeats, DateTime.Now);

            //Reserve all seats
            var reservation2 = reservationService.ReserveRangeSeats(auditorium, showtime2.Id, auditorium.Seats.Count, DateTime.Now);

            //Then the last reservation contains seats reserved from the first reservation
            reservation2.Seats.Should().HaveCount(auditorium.Seats.Count);
        }

        [Fact]
        public void Shoud_ThrowException_WhenNotEnoughSeats()
        {
            //Given a number of available seat in a auditorium
            short row = 1;
            short seatsPerRow = 10;

            var totalSeats = row * seatsPerRow;
            var auditorium = new AuditoriumEntity { Id = 1, Seats = SampleData.GenerateSeats(1, row, seatsPerRow) };

            //And given a showtime is scheduled in this auditorium
            var showtime = new ShowtimeEntity { Id = 2, AuditoriumId = auditorium.Id };
            auditorium.ScheduleShowtime(showtime);

            //When some seats are reserved
            var firstSeats = 5;
            var secondSeats = 10;
            var reservationService = new ReservationService();
            reservationService.ReserveListOfSeats(auditorium, showtime.Id, auditorium.Seats.Take(firstSeats).ToList(), DateTime.Now);

            //When trying to reserve some seats again including ones reserved before
            var act = () => reservationService.ReserveRangeSeats(auditorium, showtime.Id, secondSeats, DateTime.Now, allPlacesRequired: true);

            //Then exception should be throzn
            var exception = Assert.Throws<SeatAlreadyReservedException>(act);
            exception.Code.Should().Be(400);
            exception.Type.Should().Be("Domain.Reservation.SeatAlreadyReserved");
            exception.Message.Should().Be($"Reservation Failed for showtime({showtime.Id}). Not all seats are available");
        }

        [Fact]
        public void Shoud_ReserveMaximumContiguousSeats()
        {
            //Given a number of available seat in a auditorium
            short row = 1;
            short seatsPerRow = 10;

            var auditorium = new AuditoriumEntity { Id = 1, Seats = SampleData.GenerateSeats(1, row, seatsPerRow) };

            //And given a showtime is scheduled in this auditorium
            var showtime = new ShowtimeEntity { Id = 2, AuditoriumId = auditorium.Id };
            auditorium.ScheduleShowtime(showtime);

            //When resering and there is no enough contiguous seats and reservation is not required to contains all seats
            var firstSeats = 4;
            var secondSeats = 6;
            var alreadyReservedSeatStartIndex = 3;

            var reservationService = new ReservationService();
            reservationService.ReserveListOfSeats(auditorium, showtime.Id, auditorium.Seats.Skip(alreadyReservedSeatStartIndex - 1).Take(firstSeats).ToList(), DateTime.Now);

            var reservation2 = reservationService.ReserveRangeSeats(auditorium, showtime.Id, secondSeats, DateTime.Now);

            //Then should create a reservation with the maximum number of contiguous seats
            reservation2.Seats.Should().HaveCount(4);
        }


        [Fact]
        public void Shoud_ThrowException_WhenReservingSoldSeats()
        {
            //Given a number of available seat in a auditorium
            var auditorium = new AuditoriumEntity { Id = 1, Seats = SampleData.GenerateSeats(1, 2, 10) };

            //And given a showtime is scheduled in this auditorium
            var showtime = new ShowtimeEntity { Id = 2, AuditoriumId = auditorium.Id };
            auditorium.ScheduleShowtime(showtime);

            //And reservation is made and confirmed
            var remainingSeats = 5;
            var reservationService = new ReservationService();
            var reservation1 = reservationService.ReserveRangeSeats(auditorium, showtime.Id, auditorium.Seats.Count, DateTime.Now);
            reservationService.ConfirmReservation(auditorium, reservation1.Id);

            //When reserving the sold seats
            var act = () => reservationService.ReserveRangeSeats(auditorium, showtime.Id, 1, DateTime.Now, allPlacesRequired: true);

            //Then exception should be thron
            var exception = Assert.Throws<SeatAlreadyReservedException>(act);
            exception.Code.Should().Be(400);
            exception.Type.Should().Be("Domain.Reservation.SeatAlreadyReserved");
            exception.Message.Should().Be($"Reservation Failed for showtime({showtime.Id}). Not all seats are available");
        }
    }
}