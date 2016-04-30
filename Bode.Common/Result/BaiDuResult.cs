using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Bode.Common.Json;

namespace Bode.Common.Result
{
    public class BaiDuResult
    {
        //        {
        //  status: 0,
        //  result: {
        //    location: {
        //      lng: 116.30814954222,
        //      lat: 40.056885091681
        //    },
        //    precise: 1,
        //    confidence: 80,
        //    level: "商务大厦"
        //  }
        //}
        public class LocationResult
        {
            public int status { get; set; }
            public int precise
            {
                get;
                set;
            }
            public int confidence
            { get; set; }

            public string level { get; set; }

            public ResultBadu result { get; set; }
        }

        public class ResultBadu
        {
            public Location location { get; set; }
        }

        public class Location
        {
            public float lng { get; set; }
            public float lat { get; set; }
        }

        private static string BaiduAk = ConfigurationManager.AppSettings["baiduAk"];

        /// <summary>
        /// 地址解析
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static LocationResult GetLngLat(string address)
        {
            address = HttpUtility.UrlEncode(address);
            var url =
                "http://api.map.baidu.com/geocoder/v2/?address=" + address + "&output=json&ak=" + BaiduAk;
            var result = JsonTo.JsonStringToObject<LocationResult>(GetUrl(url));
            return result;
        }

        /// <summary>
        /// 私有get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string GetUrl(string url)
        {
            WebRequest myWebRequest = WebRequest.Create(url);

            WebResponse myWebResponse = myWebRequest.GetResponse();
            Stream ReceiveStream = myWebResponse.GetResponseStream();
            string responseStr = "";
            if (ReceiveStream != null)
            {
                StreamReader reader = new StreamReader(ReceiveStream, Encoding.UTF8);
                responseStr = reader.ReadToEnd();
                reader.Close();
            }
            myWebResponse.Close();
            return responseStr;
        }
    }
}
