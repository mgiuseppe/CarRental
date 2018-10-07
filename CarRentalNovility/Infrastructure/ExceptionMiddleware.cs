using CarRentalNovility.Entities.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
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
            catch (Exception ex) //doesn't catch 404 or other http errors thrown in controllers because they are not exceptions.
            {
                var internalErrorCode = (ex as CustomException)?.code ?? ErrorCode.GenericException;
                
                //set status code
                httpContext.Response.StatusCode = (int) internalErrorCode.ToHttpStatusCode();
                
                //log error
                logger.LogError(ex, $"Response status code {httpContext.Response.StatusCode}" + Environment.NewLine +
                                    $"request path {httpContext.Request.Path}" + Environment.NewLine +
                                    $"Request querystring: {httpContext.Request.QueryString}" + Environment.NewLine// +
                                    //$"Request body: {httpContext.Request.GetBodyAsString()}"
                                    );
                
                //write details in the http response
                var errorDetails = new ErrorDetails() { StatusCode = internalErrorCode, Message = ex.Message }.ToString();
                await httpContext.Response.WriteAsync(errorDetails);
            }
        }
    }
}
