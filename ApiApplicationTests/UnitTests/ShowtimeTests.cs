using ApiApplication.Database.Entities;
using ApiApplication.Model.Factories;
using ApiApplication.Services;
using AutoFixture;
using FluentAssertions;
using Moq;
using Moq.AutoMock;

namespace ApiApplicationTests.UnitTests
{
    public class ShowtimeTests
    {
        [Fact]
        public async Task Should_CreateShowtime()
        {
            //Given movie provider return requested movie with the correct id
            var movieId = "123";
            var auditoriumId = 2;
            var sessionDate = DateTime.Now;

            var fixture = new Fixture();
            fixture.Customize<MovieEntity>(c => c
                .With(m => m.ImdbId, movieId)
                .Without(m => m.Showtimes));
            var movie = fixture.Create<MovieEntity>();

            //When
            ShowtimeEntity showtime = ShowtimeEntity.CreateShowtime(movie, auditoriumId, sessionDate);

            //Then
            showtime.Movie.Title.Should().Be(movie.Title);
            showtime.Movie.ImdbId.Should().Be(movieId);
            showtime.Movie.Stars.Should().Be(movie.Stars);
            showtime.Movie.ReleaseDate.Should().Be(movie.ReleaseDate);
            showtime.SessionDate.Should().Be(sessionDate);
            showtime.AuditoriumId.Should().Be(auditoriumId);
        }
    }
}