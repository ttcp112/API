﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.Model
{
    public class Warning
    {
        [DataMember(EmitDefaultValue = false)]
        public string Message;
    }
}