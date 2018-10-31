using PiOne.Api.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Common
{
    public abstract class NSResponse<T> : NSResponse, INSResponse<T> {

        public abstract IList<T> Values { get; }
    }
    public abstract class NSResponse
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Status { get; set; }
    }
}
