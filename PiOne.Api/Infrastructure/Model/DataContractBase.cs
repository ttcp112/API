using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.Model
{
    [DataContract(Namespace = "")]
    public class DataContractBase
    {
        [DataMember(EmitDefaultValue = false)]
        public List<ValidationError> ValidationErrors { get; set; }
    }
}
