using PiOne.Api.Core.Response;
using PiOne.Api.Infrastructure.Exceptions;
using PiOne.Api.Infrastructure.Model;
using System;

namespace PiOne.Api.Business
{
    public class NSBusBase : IBusiness
    {
        public void EntityValidationException(ref Core.Response.NSApiResponse response, System.Data.Entity.Validation.DbEntityValidationException ex)
        {
            response.Error = new NSApiErrorResponse();

            var vEx = new ValidationException();
            foreach (var entityErr in ex.EntityValidationErrors)
            {
                foreach (var error in entityErr.ValidationErrors)
                {
                    vEx.ValidationErrors.Add(new ValidationError()
                    {
                        Message = error.ErrorMessage,
                        Type = entityErr.Entry.Entity
                    });
                }
            }
            response.Error.ExceptionData.ValidatedEx = vEx;
            response.Error.ExceptionData.Errors = vEx.ValidationErrors.ToArray();
        }

        public void ValidationException(ref Core.Response.NSApiResponse response, Exception ex)
        {
            response.Error = new NSApiErrorResponse();
            response.Error.Code = ex.GetHashCode().ToString();
            response.Error.Description = ex.Message;
            response.Error.ExceptionData.InnerEx = ex;
        }
    }
}
