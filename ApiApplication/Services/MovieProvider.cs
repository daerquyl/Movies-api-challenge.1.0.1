using ApiApplication.Domain.Models;
using ApiApplication.Exceptions;
using Grpc.Core;
using ProtoDefinitions;
using System;
using System.Threading.Tasks;

namespace ApiApplication.Services
{
    public class GrpcMovieProvider : IMovieProvider
    {
        private readonly IApiClient apiClient;

        public GrpcMovieProvider(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<MovieEntity> GetMovie(string movieId)
        {
            try
            {
                showResponse movie = await apiClient.GetMovie(movieId);
                return ToMovieEntity(movie);
            }
            catch (RpcException ex)
            {
                if (ex.StatusCode == StatusCode.NotFound)
                {
                    throw new MovieNotFoundException(movieId);
                }
                else if(ex.StatusCode == StatusCode.Unauthenticated)
                {
                    throw RemoteApiException.Create(RemoteApiExceptionType.UNAUTHORIZED);
                }
                else if (ex.StatusCode == StatusCode.Unavailable)
                {
                    throw RemoteApiException.Create(RemoteApiExceptionType.SERVICE_UNAVAILABLE);
                }
                else if (ex.StatusCode == StatusCode.DeadlineExceeded)
                {
                    throw RemoteApiException.Create(RemoteApiExceptionType.SERVER_TIMEOUT);
                }
                else
                {
                    throw RemoteApiException.Create(RemoteApiExceptionType.UNKNOWN_ERROR, ex.Message);
                }
            }
        }

        private MovieEntity ToMovieEntity(showResponse movie)
        {
            var imdbId = movie.Id;
            var stars = movie.ImDbRating;
            var releaseDate = new DateTime(int.Parse(movie.Year), 1, 1);
            var title = movie.FullTitle;

            return MovieEntity.Create(title, releaseDate, stars, imdbId);
        }
    }
}
