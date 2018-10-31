using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{
    public class ListError : NSApiResponseBase
    {
        public List<PropertyRequired> ListProperty { get; set; }

        public ListError()
        {
            ListProperty = new List<PropertyRequired>();
        }
    }

    public class PropertyRequired
    {
        public int Index { get; set; }
        public string StoreName { get; set; }
        public string Property { get; set; }
        public string Error { get; set; }

        public PropertyRequired(int index, string storeName, string property, string error)
        {
            Index = index;
            StoreName = storeName;
            Property = property;
            Error = error;
        }
    }
}
