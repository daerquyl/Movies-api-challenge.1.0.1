﻿using ApiApplication.Exceptions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using ApiApplication.Domain.Exceptions;
using Newtonsoft.Json;

namespace ApiApplication.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = exception switch
            {
                DomainException domainException => domainException.Code,
                _ => StatusCodes.Status500InternalServerError
            };

            if (exception is BaseException ex)
            {
                context.Response.ContentType = "application/json";
                var json = JsonConvert.SerializeObject(ex.ToProblem());
                await context.Response.WriteAsync(json);
            }
            else
            {
                var problem = new Problem("Unknown error, we are investigating.", StatusCodes.Status500InternalServerError, "Internal");
                var json = JsonConvert.SerializeObject(problem);
                await context.Response.WriteAsync(json);
            }

        }
    }
}
