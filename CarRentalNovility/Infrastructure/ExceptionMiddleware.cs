using CarRentalNovility.Entities.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalNovility.Web.Infrastructure
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                //log error
                logger.LogError(ex, $"Response status code {httpContext.Response.StatusCode}" + Environment.NewLine +
                                    $"request path {httpContext.Request.Path}" + Environment.NewLine +
                                    $"Request querystring: {httpContext.Request.QueryString}" + Environment.NewLine// +
                                    //$"Request body: {httpContext.Request.GetBodyAsString()}"
                                    );

                //write details in the http response
                var errorDetails = new ErrorDetails() { StatusCode = (ex as CustomException)?.code ?? ErrorCode.GenericException, Message = ex.Message }.ToString();
                await httpContext.Response.WriteAsync(errorDetails);
            }
        }
    }
}
