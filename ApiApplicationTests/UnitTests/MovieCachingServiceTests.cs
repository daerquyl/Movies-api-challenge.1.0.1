
using ApiApplication.Services;
using AutoFixture;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using ProtoDefinitions;

namespace ApiApplicationTests.UnitTests
{
    public class MovieCachingServiceTests
    {
        [Fact]
        public async Task Should_ReturnCachedValue_WhenAvailable()
        {
            //Given 
            var movieId = "key";

            var fixture = new Fixture();
            fixture.Customize<showResponse>(c => c
                .With(s => s.Id, movieId)
                .With(s => s.Year, "2023"));

            var expectedMovie = fixture.Create<showResponse>();

            //And Cache contains the request data
            var mocker = new AutoMocker();

            mocker.GetMock<ICachingService>()
                .Setup(c => c.TryGetAsync<showResponse>(movieId))
                .ReturnsAsync(expectedMovie);

            var movieProvider = mocker.CreateInstance<GrpcMovieProvider>();

            mocker.GetMock<IApiClient>()
                .Verify(api => api.GetMovie(It.IsAny<string>()), Times.Never);

            //When requesting the value
            var actualMovie = await movieProvider.GetMovie(movieId);

            //Then should get the value from the cache
            actualMovie.ImdbId.Should().Be(movieId);
        }

        [Fact]
        public async Task Should_ReturnApiValue_WhenUnavailable()
        {
            //Given 
            var movieId = "key";

            var fixture = new Fixture();
            fixture.Customize<showResponse>(c => c
                .With(s => s.Id, movieId)
                .With(s => s.Year, "2023"));

            var expectedMovie = fixture.Create<showResponse>();

            //And Cache contains the request data and the inline provider
            var mocker = new AutoMocker();

            mocker.GetMock<ICachingService>()
                .Setup(c => c.TryGetAsync<showResponse>(movieId))
                .ReturnsAsync((showResponse)null);

            mocker.GetMock<IApiClient>()
                .Setup(api => api.GetMovie(movieId))
                .ReturnsAsync(expectedMovie);

            var movieProvider = mocker.CreateInstance<GrpcMovieProvider>();

            //When requesting the value
            var actualMovie = await movieProvider.GetMovie(movieId);

            //Then should get the value from the cache
            actualMovie.ImdbId.Should().Be(movieId);
            mocker.GetMock<IApiClient>()
                .Verify(api => api.GetMovie(It.IsAny<string>()), Times.Once);
        }
        [Fact]
        public async Task Should_SaveValue_WhenUnavailable()
        {
            //Given 
            var movieId = "key";

            var fixture = new Fixture();
            fixture.Customize<showResponse>(c => c
                .With(s => s.Id, movieId)
                .With(s => s.Year, "2023"));

            var expectedMovie = fixture.Create<showResponse>();

            //And Cache contains the request data and the inline provider
            var mocker = new AutoMocker();

            mocker.GetMock<ICachingService>()
                .Setup(c => c.TryGetAsync<showResponse>(movieId))
                .ReturnsAsync((showResponse)null);

            mocker.GetMock<IApiClient>()
                .Setup(api => api.GetMovie(movieId))
                .ReturnsAsync(expectedMovie);

            var movieProvider = mocker.CreateInstance<GrpcMovieProvider>();

            //When requesting the value
            var actualMovie = await movieProvider.GetMovie(movieId);

            //Then should get the value from the cache
            mocker.GetMock<ICachingService>()
                .Verify(cache => cache.SetAsync<showResponse>(movieId, expectedMovie), Times.Once);
        }
    }
}
