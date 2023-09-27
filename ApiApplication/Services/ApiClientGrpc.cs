using System;
using System.Net.Http;
using System.Threading.Tasks;
using ApiApplication.Exceptions;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using ProtoDefinitions;

namespace ApiApplication.Services
{
    public class ApiClientGrpc : IApiClient
    {
        private GrpcChannel channel;
        private Metadata headers;

        private const int DEFAULT_TIMEOUT = 60;

        public ApiClientGrpc(IConfiguration config)
        {
            var apiUri = config.GetSection("ProvidedApi:Uri")?.Value;
            var authorizationHeaderName = config.GetSection("ProvidedApi:Credentials:Key")?.Value;
            var authorizationHeaderValue = config.GetSection("ProvidedApi:Credentials:Value")?.Value;

            if(string.IsNullOrEmpty(apiUri) 
                || string.IsNullOrEmpty(authorizationHeaderName) 
                || string.IsNullOrEmpty(authorizationHeaderValue)) 
            {
                throw new Exception("Movies API Configuration is missing.");
            }

            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            channel = GrpcChannel.ForAddress(apiUri, new GrpcChannelOptions()
            {
                HttpHandler = httpHandler,
            });

            headers = new Metadata
            {
                { authorizationHeaderName, authorizationHeaderValue }
            };
        }

        public async Task<showListResponse> GetAll()
        {
            var client = new MoviesApi.MoviesApiClient(channel);

            var all = await client.GetAllAsync(new Empty(), headers, DateTime.Now.AddSeconds(DEFAULT_TIMEOUT));
            all.Data.TryUnpack<showListResponse>(out var data);
            return data;
        }

        public async Task<showResponse> GetMovie(string movieId)
        {
            var client = new MoviesApi.MoviesApiClient(channel);

            var idRequest = new IdRequest
            {
                Id = movieId
            };

            var response = await client.GetByIdAsync(idRequest, headers);
            response.Data.TryUnpack<showResponse>(out var data);
            return data;
        }
    }
}