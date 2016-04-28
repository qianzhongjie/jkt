using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Bode.Common.Json;

namespace Bode.Common.Result
{
    public class WxResult
    {
        public static string Appid = ConfigurationManager.AppSettings["wxappid"];
        public static string Secret = ConfigurationManager.AppSettings["wxappsecret"];



        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public static TokenResponse GetToken()
        {
            var url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + Appid + "&secret=" + Secret;
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
            return JsonTo.JsonStringToObject<TokenResponse>(responseStr);
        }
        /// <summary>
        /// 获取用户信息(基础接口)
        /// </summary>
        /// <param name="token">调用接口凭据</param>
        /// <param name="openId">用户id</param>
        /// <returns></returns>
        public static UserInfoRsponse GetUserInfo(string token, string openId)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + token + "&openid=" + openId + "&lang=zh_CN";
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
            return JsonTo.JsonStringToObject<UserInfoRsponse>(responseStr);
        }

        public class UserInfoRsponse
        {
            public string openId { get; set; }

            public string nickName { get; set; }

            public string sex { get; set; }

            public string province { get; set; }

            public string city { get; set; }

            public string country { get; set; }

            //  [JsonProperty("headimgurl")]
            public string headimgurl { get; set; }
            public string errcode { get; set; }
            public string errmsg { get; set; }

            //[JsonProperty("privilege")]
            public IEnumerable<string> privilege { get; set; }
        }

        public class TokenResponse
        {
            public string access_token { get; set; }
            public string expires_in { get; set; }
            public string errcode { get; set; }
            public string errmsg { get; set; }

        }
    }

}
