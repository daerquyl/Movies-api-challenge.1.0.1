using ApiApplication.Domain.Exceptions;
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
        private readonly ICachingService cache;

        public GrpcMovieProvider(IApiClient apiClient, ICachingService cache)
        {
            this.apiClient = apiClient;
            this.cache = cache;
        }

        public async Task<MovieEntity> GetMovie(string movieId)
        {
            var movie = await TryGetFromCache(movieId);
            if (movie is null)
            {
                movie = await GetFromApi(movieId);
                await TrySetToCache(movieId, movie);
            }

            if(movie is null)
            {
                throw new MovieNotFoundException(movieId);
            }
            return ToMovieEntity(movie);
        }

        private async Task TrySetToCache(string movieId, showResponse movie)
        {
            try
            {
                await cache.SetAsync(movieId, movie);
            }catch(Exception ex)
            {
                //Log exception here
            }
        }

        private async Task<showResponse> TryGetFromCache(string movieId)
        {
            try
            {
                var response = await cache.TryGetAsync<showResponse>(movieId);
                if (!(response is null))
                {
                    var movie = (showResponse) response;
                }
            }
            catch (Exception ex)
            {
                //Should log the exception here 
            }

            return null;
        }

        private async Task<showResponse> GetFromApi(string movieId)
        {
            try
            {
                showResponse movie = await apiClient.GetMovie(movieId);
                return movie;
            }
            catch (RpcException ex)
            {
                if (ex.StatusCode == StatusCode.NotFound)
                {
                    throw new MovieNotFoundException(movieId);
                }
                else if (ex.StatusCode == StatusCode.Unauthenticated)
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
