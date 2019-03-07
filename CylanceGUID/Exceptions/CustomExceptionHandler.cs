using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CylanceGUID.Exceptions
{
    public class CustomExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode statusCode;

            //check if context is a WebException
            if(context.Exception is WebException && (context.Exception as WebException).Response !=null)
            {
                statusCode = ((context.Exception as WebException).Response as HttpWebResponse).StatusCode;
            }
            else
            {
                statusCode = GetErrorCode(context.Exception.GetType());
            }

            string errMsg = context.Exception.Message;
            

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)statusCode;
            response.ContentType = "application/json";

            var result = JsonConvert.SerializeObject(new
                            {
                                errorMessage = errMsg,
                                errorCode = statusCode
                            });

            response.ContentLength = result.Length;
            response.WriteAsync(result);

        }

        private HttpStatusCode GetErrorCode(Type exceptionType)
        {
            ExceptionEnum parseException;
            if(Enum.TryParse(exceptionType.Name, out parseException))
            {
                switch(parseException)
                {
                    case ExceptionEnum.RecordNotFound:
                        return HttpStatusCode.NotFound;
                    case ExceptionEnum.InvalidRequestParameter:
                        return HttpStatusCode.BadRequest;
                    case ExceptionEnum.RecordAlreadyExists:
                        return HttpStatusCode.BadRequest;
                    default:
                        return HttpStatusCode.InternalServerError;
                }
            }
            else
                return HttpStatusCode.InternalServerError;

        }
    }
}
