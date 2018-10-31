using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.Model
{
    [DataContract(Namespace = "")]
    public class ValidationError
    {
        [DataMember(EmitDefaultValue = false)]
        public string Message { get; set; }

        public object Type { get; set; }
    }
}
