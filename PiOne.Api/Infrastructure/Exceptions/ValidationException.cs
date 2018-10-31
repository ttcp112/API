using PiOne.Api.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.Exceptions
{
    public class ValidationException
        : BadRequestException
    {
        public ValidationException() { ValidationErrors = new List<ValidationError>(); }
        public ValidationException(ApiException apiException)
            : base(apiException)
        {
            if (apiException.Elements != null && apiException.Elements.Any())
            {
                ValidationErrors = new List<ValidationError>();
                foreach (var ve in apiException
                    .Elements
                    .Where(x => x.ValidationErrors != null)
                    .SelectMany(e => e.ValidationErrors)) { ValidationErrors.Add(ve); }
            }
        }

        public List<ValidationError> ValidationErrors { get; set; }

        public override string ToString()
        {
            return string.Join("\n", ValidationErrors.Select(p => p.Message));
        }
    }

}
