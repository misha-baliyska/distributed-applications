﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TM.Infrastructure.Messaging
{
    public class ServiceResponseError: ServiceResponseBase
    {
        [JsonIgnore]
        public string? DeveloperError { get; set; }

        required public string Message { get; set; }

        public ServiceResponseError() : base(BusinessStatusCodeEnum.InternalServerError) { }
    }
}
