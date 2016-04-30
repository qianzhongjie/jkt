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
        /// 获取openID
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static OpenIdResponse GetOpenId(string code)
        {
            var url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + Appid + "&secret=" + Secret +
                      "&code=" + code + "&grant_type=authorization_code";
            return JsonTo.JsonStringToObject<OpenIdResponse>(GetUrl(url));
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public static TokenResponse GetToken()
        {
            var url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + Appid + "&secret=" +
                      Secret;
            return JsonTo.JsonStringToObject<TokenResponse>(GetUrl(url));
        }

        /// <summary>
        /// 获取用户信息(基础接口)
        /// </summary>
        /// <param name="token">调用接口凭据</param>
        /// <param name="openId">用户id</param>
        /// <returns></returns>
        public static UserInfoRsponse GetUserInfo(string token, string openId)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + token + "&openid=" + openId +
                      "&lang=zh_CN";
            return JsonTo.JsonStringToObject<UserInfoRsponse>(GetUrl(url));
        }

        /// <summary>
        /// 获取网页api_ticket
        /// </summary>
        /// <returns></returns>
        public static JsapiTicket GetTicket(string token)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + token + "&type=jsapi";
            return JsonTo.JsonStringToObject<JsapiTicket>(GetUrl(url));
        }

        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
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

        public static string GetString(int count)
        {
            int number;
            string checkCode = String.Empty;     //存放随机码的字符串   

            System.Random random = new Random();

            for (int i = 0; i < count; i++) //产生4位校验码   
            {
                number = random.Next();
                number = number % 36;
                if (number < 10)
                {
                    number += 48;    //数字0-9编码在48-57   
                }
                else
                {
                    number += 55;    //字母A-Z编码在65-90   
                }

                checkCode += ((char)number).ToString();
            }
            return checkCode;
        }

        /// <summary>
        /// userinfoModel
        /// </summary>
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

        /// <summary>
        /// tokenModel
        /// </summary>
        public class TokenResponse
        {
            public string access_token { get; set; }
            public string expires_in { get; set; }
            public string errcode { get; set; }
            public string errmsg { get; set; }

        }

        /// <summary>
        /// openIdmodel
        /// </summary>
        public class OpenIdResponse
        {
            public string access_token { get; set; }
            public string expires_in { get; set; }
            public string refresh_token { get; set; }
            public string openid { get; set; }
            public string scope { get; set; }
            public string errcode { get; set; }
            public string errmsg { get; set; }
        }

        /// <summary>
        /// ticketMode
        /// </summary>
        public class JsapiTicket : ErrModel
        {

            public string ticket { get; set; }

        }

        /// <summary>
        /// errorModel
        /// </summary>
        public class ErrModel
        {
            public string expires_in { get; set; }
            public string errcode { get; set; }
            public string errmsg { get; set; }
        }
    }
}


