using PiOne.Api.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Common
{
    [DataContract(Namespace = "")]
    public abstract class CoreData
    {
        [DataMember(EmitDefaultValue=false, Name="ValidationErrors")]
        public List<ValidationError> Errors { get;set; }

        [DataMember(EmitDefaultValue=false, Name= "Warnings")]
        public List<Warning> Warnings { get;set; }
    }
}
