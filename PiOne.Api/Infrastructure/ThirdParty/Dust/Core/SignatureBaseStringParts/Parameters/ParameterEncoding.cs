using System;
using PiOne.Api.Infrastructure.ThirdParty.HttpUtility;

namespace PiOne.Api.Infrastructure.ThirdParty.Dust.Core.SignatureBaseStringParts.Parameters
{
    internal class ParameterEncoding
    {
        internal string Escape(string what)
        {
            return UrlEncoder.UrlEncode(what ?? string.Empty);
        }
    }
}