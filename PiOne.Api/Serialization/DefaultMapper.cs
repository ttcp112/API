using PiOne.Api.Infrastructure.Interfaces;
using PiOne.Api.Infrastructure.ThirdParty.ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Serialization
{
    public class DefaultMapper: IJsonObjectMapper, IXmlObjectMapper
    {
        public DefaultMapper() { }

        T IJsonObjectMapper.From<T>(string result)
        {
            var item = JsonSerializer.DeserializeFromString<T>(result);
            return item;
        }

        string IJsonObjectMapper.To<T>(T request)
        {
            var json = JsonSerializer.SerializeToString(request);
            return json;
        }

        T IXmlObjectMapper.From<T>(string result)
        {
            var item = XmlSerializer.DeserializeFromString<T>(result);
            return item;
        }

        string IXmlObjectMapper.To<T>(T request)
        {
            string xml = XmlSerializer.SerializeToString(request);
            return xml;
        }

        private void BuildEnumList()
        {
            BuildCore();
        }

        private void BuildCore() { }

        private static TEnum EnumDeserializer<TEnum>(string s)
            where TEnum : struct
        {
            return EnumDeserializerRun<TEnum>(s);
        }

        private static TEnum? EnumDeserializerNullable<TEnum>(string s)
            where TEnum : struct
        {
            // first off, is this an empty string? 
            if (String.IsNullOrEmpty(s))
            {
                // ... then just return null
                return null;
            }
            return EnumDeserializerRun<TEnum>(s);
        }

        private static TEnum EnumDeserializerRun<TEnum>(string s) where TEnum : struct
        {
            TEnum t;

            // If all goes well then we are done
            if (Enum.TryParse(s, out t))
                return t;

            // get the EnumMember attribute and see if the Value attribute matches the string
            foreach (var p in t.GetType().GetMembers())
            {
                var attributes = p.GetCustomAttributes(typeof(EnumMemberAttribute), false).Cast<EnumMemberAttribute>();
                if (attributes.All(a => String.Compare(a.Value, s, StringComparison.OrdinalIgnoreCase) != 0))
                    continue;
                Enum.TryParse(p.Name, out t);
            }

            return t;
        }

    }
}
