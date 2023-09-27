using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Services;
using ApiApplication.UseCases.Showtimes.CreateShowtime;
using AutoFixture;
using Moq;
using Moq.AutoMock;
using FluentAssertions;
using ApiApplication.Exceptions;

namespace ApiApplicationTests.UnitTests
{
    public class ShowtimeCommandTests
    {
        [Fact]
        public async Task Handle_ValidCreateShowtimeRequest_ShouldCreateShowtime()
        {
            var movieId = "123";
            var auditoriumId = 1;
            var sessionDate = DateTime.Now.AddDays(1);

            //Given a movie entitity is returned by Provived API
            var mocker = new AutoMocker();

            var fixture = new Fixture();
            fixture.Customize<MovieEntity>(c => c
                .Without(m => m.Showtimes)
                .Without(m => m.Id));
            var movieEntity = fixture.Create<MovieEntity>();

            mocker.GetMock<IMovieProvider>()
                .Setup(provider => provider.GetMovie(movieId))
                .ReturnsAsync(movieEntity);

            mocker.GetMock<IAuditoriumsRepository>()
                .Setup(repo => repo.GetAsync(auditoriumId, CancellationToken.None))
                .ReturnsAsync(new AuditoriumEntity());

            //And a valid create swhow time command
            var command = new CreateShowtimeCommand
            {
                MovieId = movieId,
                AuditoriumId = auditoriumId,
                SessionDate = sessionDate
            };

            // When command handler is called
            var handler = mocker.CreateInstance<CreateShowtimeCommandHandler>();
            await handler.Handle(command, CancellationToken.None);

            //A new Showtime entity should be created and persisted
            fixture.Customize<ShowtimeEntity>(c => c
                .With(m => m.Movie, movieEntity)
                .With(m => m.AuditoriumId, auditoriumId)
                .With(m => m.SessionDate, sessionDate)
                .Without(m => m.Tickets)
                .Without(m => m.Id));
            var newShowtimeEntity = fixture.Create<ShowtimeEntity>();

            mocker.GetMock<IShowtimesRepository>()
            .Verify(r => r.CreateShowtime(
                It.Is<ShowtimeEntity>(showtime =>
                    showtime.Movie.ImdbId == movieEntity.ImdbId &&
                    showtime.SessionDate == command.SessionDate &&
                    showtime.AuditoriumId == command.AuditoriumId
                ),
                CancellationToken.None
            ), Times.Once);
        }

        [Fact]
        public async Task Handle_CreateShowtimeRequestWithInvalidRequest_ShouldThrowException()
        {
            //Given create showtime request with sessionDate in past
            var movieId = "123";
            var auditoriumId = 1;
            var sessionDate = DateTime.Now.AddMinutes(-1);

            var command = new CreateShowtimeCommand
            {
                MovieId = movieId,
                AuditoriumId = auditoriumId,
                SessionDate = sessionDate
            };

            var mocker = new AutoMocker();

            // When command handler is called
            var handler = mocker.CreateInstance<CreateShowtimeCommandHandler>();

            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            var exception = Assert.ThrowsAsync<UserInputValidationException>(act).Result;
            exception.Code.Should().Be(400);
            exception.Type.Should().Be("InvalidOperation.CreateShowtime.WrongDate");
            exception.Message.Should().Be("Cannot create showtime in past");
        }

        [Fact]
        public async Task Handle_CreateShowtimeRequestWithInvalidAuditoriumId_ShouldThrowException()
        {
            //Given create showtime request with unknow AuditoriumId
            var movieId = "123";
            var auditoriumId = 1;
            var sessionDate = DateTime.Now.AddDays(1);

            var command = new CreateShowtimeCommand
            {
                MovieId = movieId,
                AuditoriumId = auditoriumId,
                SessionDate = sessionDate
            };

            var mocker = new AutoMocker();
            mocker.GetMock<IAuditoriumsRepository>()
                .Setup(repo => repo.GetAsync(auditoriumId, CancellationToken.None))
                .ReturnsAsync((AuditoriumEntity)null);

            // When command handler is called
            var handler = mocker.CreateInstance<CreateShowtimeCommandHandler>();

            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            var exception = Assert.ThrowsAsync<UserInputValidationException>(act).Result;
            exception.Code.Should().Be(400);
            exception.Type.Should().Be("InvalidOperation.CreateShowtime.UnknowAuditorium");
            exception.Message.Should().Be("Cannot create showtime without in a non valid auditorium");
        }

        [Fact]
        public async Task Handle_CreateShowtimeRequestForAlreadySavedMovie_ShouldNotCallProvidedApi()
        {
            //Given create showtime request for an already retrieved movie
            var movieId = "123";
            var auditoriumId = 1;
            var sessionDate = DateTime.Now.AddDays(1);

            var command = new CreateShowtimeCommand
            {
                MovieId = movieId,
                AuditoriumId = auditoriumId,
                SessionDate = sessionDate
            };

            var mocker = new AutoMocker();
            mocker.GetMock<ILocalMovieRepository>()
                .Setup(repo => repo.GetByImdbIdAsync(movieId, CancellationToken.None))
                .ReturnsAsync(new MovieEntity());

            mocker.GetMock<IAuditoriumsRepository>()
                .Setup(repo => repo.GetAsync(auditoriumId, CancellationToken.None))
                .ReturnsAsync(new AuditoriumEntity());

            // When command handler is called
            var handler = mocker.CreateInstance<CreateShowtimeCommandHandler>();
            await handler.Handle(command, CancellationToken.None);

            mocker.GetMock<IMovieProvider>()
            .Verify(p => p.GetMovie(movieId), Times.Never);
        }
    }
}
