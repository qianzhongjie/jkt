using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WX.Model.ApiResponses
{
    public class SnsUserInfoResponse : ApiResponse
    {
        public string openId { get; set; }

        public string nickName { get; set; }

        public string sex { get; set; }

        public string province { get; set; }

        public string city { get; set; }

        public string country { get; set; }

        [JsonProperty("headimgurl")]
        public string headImageUrl { get; set; }

        [JsonProperty("privilege")]
        public IEnumerable<string> privilege { get; set; }
    }
}
