﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WX.Model.ApiResponses
{
    public class DatacubeGetUserCumulateResponse : ApiResponse
    {
        [JsonProperty("list")]
        public IEnumerable<UserDatacube> List { get; set; }
    }
}
