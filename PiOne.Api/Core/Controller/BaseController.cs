using PiOne.Api.Common;
using PiOne.Api.Core.Request;
using PiOne.Api.Core.Response;
using PiOne.Api.Infrastructure.Exceptions;
using PiOne.Api.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace PiOne.Api.Core.Controller
{
    public class BaseController : ApiController
    {
        protected bool Validate(RequestModelBase input)
        {
            NSLog.Logger.Info(Request.RequestUri.LocalPath, input);
            return ModelState.IsValid && input != null;
        }

        protected HttpResponseMessage ResponseForNotAvailable(Exception ex)
        {
            NSLog.Logger.Error("ResponseForNotAvailable", ex);
            return Request.CreateResponse<NotAvailableException>(HttpStatusCode.ServiceUnavailable, new NotAvailableException() { });
        }

        protected NSApiResponse ResponseForValidationFailed(ModelStateDictionary modelState)
        {
            if (modelState != null && modelState.Count > 0)
            {
                var vEx = new ValidationException()
                {
                    ValidationErrors = new List<ValidationError>()
                };
                var errors = modelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    vEx.ValidationErrors.Add(new ValidationError()
                    {
                        Message = string.Format("{0}", !string.IsNullOrEmpty(error.ErrorMessage) ? error.ErrorMessage : error.Exception.Message)
                    });
                }
                return new NSApiResponse()
                {
                    Error = new NSApiErrorResponse()
                    {
                        ExceptionData = new ExceptionResponse()
                        {
                            ValidatedEx = vEx
                        }
                    },
                };
            }
            return new NSApiResponse()
            {
                Message = "Invalid posted data"
            };
        }

        protected HttpResponseMessage ResponseForUnAuthorized()
        {
            return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Provided APPID or AccessToken is invalid");
        }

        protected NSApiResponse ResponseForInvalidAppRegistered()
        {
            return new NSApiResponse()
            {
                Message = "Invalid App Registered",
                Error = new NSApiErrorResponse()
                {
                    Code = Constants.IVARC,
                    Description = "AppKey/AppSecret is empty. Contact Service Provider for more information.",
                }
            };
        }

        protected NSApiResponse ResponseForOrganizationNotExist()
        {
            return new NSApiResponse()
            {
                Message = "Organization does not exist",
                Error = new NSApiErrorResponse()
                {
                    Code = Constants.CNEX,
                    Description = "The Organization does not exist to create new company."
                }
            };
        }

        protected bool InvalidAppRegistered(string appKey, string appSecret)
        {
            if (String.IsNullOrEmpty(appKey) || String.IsNullOrEmpty(appSecret))
            {
                return true;
            }
            return false;
        }
    }
}
