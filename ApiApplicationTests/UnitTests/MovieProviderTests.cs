
using ApiApplication.Exceptions;
using ApiApplication.Services;
using AutoFixture;
using FluentAssertions;
using Grpc.Core;
using Moq;
using Moq.AutoMock;
using ProtoDefinitions;

namespace ApiApplicationTests.UnitTests
{
    public class MovieProviderTests
    {
        [Fact]
        public async Task Should_ReturnMovieEntity()
        {
            //Provided that api client send a valid movie
            var movieId = "123";
            var fixture = new Fixture();
            fixture.Customize<showResponse>(composer =>
            {
                return composer
                    .With(x => x.Id, movieId)
                    .With(x => x.FullTitle, "Mock Full Title")
                    .With(x => x.Year, "2022")
                    .With(x => x.ImDbRating, "9.0");
            });
            var movieResponse = fixture.Create<showResponse>();


            var mocker = new AutoMocker();
            var movieProvider = mocker.CreateInstance<GrpcMovieProvider>();
            mocker.GetMock<IApiClient>()
                .Setup(api => api.GetMovie(movieId))
                .ReturnsAsync(movieResponse);

            //When a movie is requested
            var movie = await movieProvider.GetMovie(movieId);

            //Movie should have the value expected
            movie.ImdbId.Should().Be(movieResponse.Id);
            movie.Title.Should().Be(movieResponse.FullTitle);
            movie.ReleaseDate.Should().Be(new DateTime(int.Parse(movieResponse.Year), 1, 1));
            movie.Stars.Should().Be(movieResponse.ImDbRating);
        }

        [Fact]
        public async Task Should_ThrowMovieNotFoundException_WhenMovieDoesntExists()
        {
            //Provided that api client send nothing
            var movieId = "123";

            var mocker = new AutoMocker();
            var movieProvider = mocker.CreateInstance<GrpcMovieProvider>();
            mocker.GetMock<IApiClient>()
                .Setup(api => api.GetMovie(movieId))
                .Throws(new RpcException(new Status(StatusCode.NotFound, string.Empty)));

            Func<Task> act = async () => await movieProvider.GetMovie(movieId);

            var exception = Assert.ThrowsAsync<MovieNotFoundException>(act).Result;
            exception.Message.Should().Be($"Movie with ImdbId({movieId}) does'nt exist");
            exception.Code.Should().Be(404);
            exception.Type.Should().Be("ProvidedAPI.MovieNotFound");
        }

        [Fact]
        public async Task Should_ThrowRemoteApiException_WhenProvivedApiIsUnavailable()
        {
            //Provided that api client send nothing
            var movieId = "123";

            var mocker = new AutoMocker();
            var movieProvider = mocker.CreateInstance<GrpcMovieProvider>();
            mocker.GetMock<IApiClient>()
                .Setup(api => api.GetMovie(movieId))
                .Throws(new RpcException(new Status(StatusCode.Unavailable, string.Empty)));

            Func<Task> act = async () => await movieProvider.GetMovie(movieId);

            var exception = Assert.ThrowsAsync<RemoteApiException>(act).Result;
            exception.Code.Should().Be(503);
            exception.Type.Should().Be("ProvidedAPI.ServiceUnavailable");
            exception.Message.Should().Be("Service Temporary unavailable");
        }


        [Fact]
        public async Task Should_ThrowRemoteApiException_WhenProvivedApiTimeout()
        {
            //Provided that api client send nothing
            var movieId = "123";

            var mocker = new AutoMocker();
            var movieProvider = mocker.CreateInstance<GrpcMovieProvider>();
            mocker.GetMock<IApiClient>()
                .Setup(api => api.GetMovie(movieId))
                .Throws(new RpcException(new Status(StatusCode.DeadlineExceeded, string.Empty)));

            Func<Task> act = async () => await movieProvider.GetMovie(movieId);

            var exception = Assert.ThrowsAsync<RemoteApiException>(act).Result;
            exception.Code.Should().Be(504);
            exception.Type.Should().Be("ProvidedAPI.Timeout");
            exception.Message.Should().Be("Server took too long to respond");
        }

        [Fact]
        public async Task Should_ThrowRemoteApiException_WhenProvivedApiSendOtherErrors()
        {
            //Provided that api client send nothing
            var movieId = "123";
            var exceptionMessage = "Any exception Message";

            var statuses = new StatusCode[] { StatusCode.Aborted, StatusCode.ResourceExhausted, StatusCode.Internal };
            foreach (var status in statuses)
            {
                var mocker = new AutoMocker();
                var movieProvider = mocker.CreateInstance<GrpcMovieProvider>();
                mocker.GetMock<IApiClient>()
                    .Setup(api => api.GetMovie(movieId))
                    .Throws(new RpcException(new Status(status, exceptionMessage)));

                Func<Task> act = async () => await movieProvider.GetMovie(movieId);

                var exception = Assert.ThrowsAsync<RemoteApiException>(act).Result;
                exception.Code.Should().Be(500);
                exception.Type.Should().Be("ProvidedAPI.ServiceUnavailable");
                exception.Message.Should().Contain(exceptionMessage);
            }
        }
    }
}
