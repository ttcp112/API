using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.Interfaces
{
    public interface IXmlObjectMapper
    {
        T From<T>(string result);
        string To<T>(T request);
    }
}
