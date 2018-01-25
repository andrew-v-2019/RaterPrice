using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
using RaterPrice.ViewModels.MobileApi.Models;

namespace RaterPrice.Api.Areas.MobileApi.Controllers
{
    public class BaseApiController : ApiController
    {
        protected IHttpActionResult CodedSuccessResult(int code, string message = "")
        {
            return Json(new {data = new {code, message}});
        }

        protected IHttpActionResult EmptySuccessResult()
        {
            return Json(new {data = ""});
        }

        protected IHttpActionResult SuccessResult<T>(T model)
        {
            return Json(new ExecutionResultEntity<T>(model),
                new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
        }

        protected IHttpActionResult ErrorResult(ModelStateDictionary modelState, int httpStatus = 400, int errorCode = 2)
        {
            HttpStatusCode requestStatusCode;

            if (!Enum.TryParse(httpStatus.ToString(), out requestStatusCode))
            {
                requestStatusCode = HttpStatusCode.BadRequest;
            }


            var errorResponse = new HttpResponseMessage(requestStatusCode)
            {
                RequestMessage = Request
            };

            if (modelState == null || !modelState.Any())
            {
                return ResponseMessage(errorResponse);
            }


            var modelStateErrors = modelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage).ToList();

            var resultErrorEntity = new ExecutionResultEntity<object>
            {
                error = new ExecutionErrorDetails
                {
                    Code = errorCode,
                    Message = string.Join(" ", modelStateErrors)
                }
            };

            errorResponse.Content =
                new StringContent(JsonConvert.SerializeObject(resultErrorEntity,
                    new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}));

            return ResponseMessage(errorResponse);
        }

        protected IHttpActionResult ErrorResult(int code, string message, int httpStatus = 400)
        {
            HttpStatusCode requestStatusCode;

            if (!Enum.TryParse(httpStatus.ToString(), out requestStatusCode))
            {
                requestStatusCode = HttpStatusCode.BadRequest;
            }


            var errorResponse = new HttpResponseMessage(requestStatusCode)
            {
                RequestMessage = Request
            };

            var resultErrorEntity = new ExecutionResultEntity<object>
            {
                error = new ExecutionErrorDetails
                {
                    Code = code,
                    Message = message
                }
            };

            errorResponse.Content =
                new StringContent(JsonConvert.SerializeObject(resultErrorEntity,
                    new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}));

            return ResponseMessage(errorResponse);
        }
    }
}