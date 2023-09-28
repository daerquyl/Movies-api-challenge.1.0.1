using ApiApplication.Database;
using ApiApplication.Domain.Exceptions;
using ApiApplication.Domain.Models;
using ApiApplication.Domain.services;
using FluentAssertions;

namespace ApiApplicationTests.UnitTests
{
    public class BuyingServiceTests
    {
        [Fact]
        public async Task Shoud_ConfirmReservation()
        {
            //Given a number of available seat in a auditorium
            short row = 1;
            short seatsPerRow = 10;

            var auditorium = new AuditoriumEntity { Id = 1, Seats = SampleData.GenerateSeats(1, row, seatsPerRow) };

            //And given a showtime is scheduled in this auditorium
            var showtime = new ShowtimeEntity { Id = 2, AuditoriumId = auditorium.Id };
            auditorium.ScheduleShowtime(showtime);

            //And a reservation is made
            var reservationService = new ReservationService();
            var ticket = reservationService.ReserveListOfSeats(auditorium, showtime.Id, auditorium.Seats.ToList(), DateTime.Now);

            //When reservation is bought
            reservationService.ConfirmReservation(auditorium, ticket.Id);

            //Then payment is valided
            ticket.Paid.Should().Be(true);
        }

        [Fact]
        public void Shoud_ThrowException_WhenConfirmingAlreadyConfirmReservation()
        {
            //Given a number of available seat in a auditorium
            short row = 1;
            short seatsPerRow = 10;

            var auditorium = new AuditoriumEntity { Id = 1, Seats = SampleData.GenerateSeats(1, row, seatsPerRow) };

            //And given a showtime is scheduled in this auditorium
            var showtime = new ShowtimeEntity { Id = 2, AuditoriumId = auditorium.Id };
            auditorium.ScheduleShowtime(showtime);

            //And reservation is confirmed
            var reservationService = new ReservationService();
            var ticket = reservationService.ReserveListOfSeats(auditorium, showtime.Id, auditorium.Seats.ToList(), DateTime.Now);
            reservationService.ConfirmReservation(auditorium, ticket.Id);

            //When confirming the same reservation
            var act = () => reservationService.ConfirmReservation(auditorium, ticket.Id);

            //Then Exception is Thrown
            var exception = Assert.Throws<ReservationAlreadyConfirmedException>(act);
            exception.Code.Should().Be(400);
            exception.Type.Should().Be("Domain.Reservation.AlreadyConfirmed");
            exception.Message.Should().Be($"Confirming Reservation ({ticket.Id}) failed. Reservation is already confirmed");
        }

        [Fact]
        public void Shoud_ThrowException_WhenBuyingSoldSeats()
        {
            //Given a number of available seat in a auditorium
            short row = 1;
            short seatsPerRow = 10;

            var auditorium = new AuditoriumEntity { Id = 1, Seats = SampleData.GenerateSeats(1, row, seatsPerRow) };

            //And given a showtime is scheduled in this auditorium
            var showtime = new ShowtimeEntity { Id = 2, AuditoriumId = auditorium.Id };
            auditorium.ScheduleShowtime(showtime);

            //And reservation is confirmed
            var reservationService = new ReservationService();
            var ticket1 = reservationService.ReserveListOfSeats(auditorium, showtime.Id, auditorium.Seats.ToList(), DateTime.Now.AddMinutes(-TicketEntity.RESERVATION_DURATION));
            var ticket2 = reservationService.ReserveListOfSeats(auditorium, showtime.Id, auditorium.Seats.Take(1).ToList(), DateTime.Now);
            reservationService.ConfirmReservation(auditorium, ticket1.Id);

            //When confirming the same reservation
            var act = () => reservationService.ConfirmReservation(auditorium, ticket2.Id);

            //Then Exception is Thrown
            var exception = Assert.Throws<SeatAlreadyBoughtException>(act);
            exception.Code.Should().Be(400);
            exception.Type.Should().Be("Domain.Reservation.SeatAlreadyBought");
            exception.Message.Should().Be($"Confirming Reservation showtime({ticket2.Id}) failed. Some seats are already bought.");
        }
    }
}
