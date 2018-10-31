using PiOne.Api.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.Exceptions
{
    public class ApiException
    {
        public string Message { get; set; }
        public string Type { get; set; }
        public int ErrorNumber { get; set; }

        public List<DataContractBase> Elements { get; set; }
    }
}
