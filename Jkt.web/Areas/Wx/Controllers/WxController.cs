using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bode.Services.Core.Contracts;
using Bode.Services.Core.Dtos.User;
using Bode.Services.Core.Models.Identity;
using Bode.Services.Core.Models.User;
using OSharp.Utility.Data;
using OSharp.Web.Http;
using WX.Api;
using WX.Model;
using WX.Model.ApiRequests;
using WX.Model.ApiResponses;
using WX.OAuth;
using OSharp.Utility.Logging;
using Microsoft.AspNet.Identity;

namespace Bode.Web.Areas.Wx.Controllers
{
    [Description("基类控制机")]
    public class WxController : Controller
    {
        public string Token { get; set; }
        public string OpenId { get; set; }
        public string NickName { get; set; }

        public string Host = ConfigurationManager.AppSettings["ServerHost"];
        public UserInfoRegistDto UserInfo { get; set; }
        private IApiClient m_client = new DefaultApiClient();
        private AppIdentication m_appIdent = new AppIdentication(
            ConfigurationManager.AppSettings["wxappid"],
            ConfigurationManager.AppSettings["wxappsecret"]);
        public IUserContract UserContract { get; set; }
        public WxController()
        {
            //string userAgent = Request.UserAgent;
            //if (!userAgent.ToLower().Contains("micromessenger"))
            //{
            //    Response.Write("请在微信中访问");
            //}
            //GetCode();

   
            //if (UserInfo.Id == 0)
            //{

            //}
        }

        //调用网页授权
        public void GetCode()
        {
            var manager = new OAuthHelper(ConfigurationManager.AppSettings["wxappid"]);
            var url = manager.BuildOAuthUrl(Host + "Wx/Result/AuthNotifyUrl",
                 OAuthScope.Base, "getAuth");

            //发起请求
            // m_client.Execute(url);
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
            //responseStr
        }

        //获取token和openid
        public void GetToken()
        {
            HttpCookie cookie = Request.Cookies["token"];
            if (cookie == null)
            {

                return;
            }
            var Code = cookie.Values["Code"];
            var loger = LogManager.GetLogger("RequestToken");
            var request = new SnsOAuthAccessTokenRequest
            {
                AppID = m_appIdent.AppID,
                AppSecret = m_appIdent.AppSecret,
                Code = Code
            };
            var response = m_client.Execute(request);
            loger.Error("MSG:{0};Code:{1};response:{2}", response.ErrorMessage, response.ErrorCode, response);
            if (!response.IsError)
            {
                Token = response.AccessToken;
                OpenId = response.OpenId;
            }
            else
            {

            }
        }

        //获取用户信息
        public void GetUserInfo()
        {
            SnsUserInfoRequest request = new SnsUserInfoRequest
            {
                OAuthToken = Token,
                Lang = Language.CN,
                OpenId = OpenId
            };
            SnsUserInfoResponse response = m_client.Execute(request);
            if (!response.IsError)
            {
                UserInfo = new UserInfoRegistDto
                {
                    Password = OpenId,
                    NickName = response.nickName,
                    UserName = response.openId,
                    Province = response.province,
                    City = response.city,
                    Country = response.country,
                    //RealName = response.NickName,
                    HeadPic = response.headImageUrl,
                    Sex = response.sex == "男" ? Sex.男 : Sex.女
                };
            }
            else
            {
            }
        }

        //注册用户
        public async void Re()
        {
            var loger = LogManager.GetLogger("Register");
            var result = await UserContract.ValidateRegister(UserInfo, "");
            loger.Error("MSG:{0};", result.Successed);
            if (result.Successed)
            {
                UserInfo.Id = (int)result.Data;
            }
        }
        //处理异常 跳转404页面
        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    //if (!filterContext.ExceptionHandled && filterContext.Exception is ArgumentOutOfRangeException)
        //    if (!filterContext.ExceptionHandled)
        //    {
        //        filterContext.Result = new RedirectResult("~/Content/404/404.htm");
        //        filterContext.ExceptionHandled = true;
        //    }
        //    base.OnException(filterContext);
        //}
    }
}