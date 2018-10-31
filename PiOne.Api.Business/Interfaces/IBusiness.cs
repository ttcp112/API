using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business
{
    public interface IBusiness
    {
        void EntityValidationException(ref NSApiResponse response, DbEntityValidationException ex);

        void ValidationException(ref NSApiResponse response, Exception ex);
    }
}
