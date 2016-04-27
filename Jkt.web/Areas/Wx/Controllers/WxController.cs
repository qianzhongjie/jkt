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

namespace Bode.Web.Areas.Wx.Controllers
{
    [Description("基类控制机")]
    public class WxController : FristController
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
            //GetCode();
            if (!string.IsNullOrWhiteSpace(Code))
            {
                //获取token
                GetToken();
                if (!string.IsNullOrWhiteSpace(Token) && !string.IsNullOrWhiteSpace(OpenId))
                {
                    var userInfo = UserContract.UserInfos.Where(x => x.SysUser.UserName == OpenId);
                    if (!userInfo.Any())
                    {
                        //注册用户
                        Re();
                    }
                    else
                    {
                        //跟新userId
                        var user = userInfo.First();
                        UserInfo.Id = user.Id;
                        // UserInfo.UserName = user.SysUser.UserName;
                        //UserInfo.NickName = user.SysUser.NickName;
                        UserInfo.RealName = user.RealName;
                        //UserInfo.HeadPic = user.HeadPic;
                        UserInfo.Sex = user.Sex;

                    }
                }
            }
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
            var request = new SnsOAuthAccessTokenRequest
            {
                AppID = m_appIdent.AppID,
                AppSecret = m_appIdent.AppSecret,
                Code = Code
            };
            var response = m_client.Execute(request);
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
                    Password = "dd",
                    NickName = response.NickName,
                    UserName = response.OpenId,
                    Province = response.Province,
                    City = response.City,
                    Country = response.Country,
                    //RealName = response.NickName,
                    HeadPic = response.HeadImageUrl,
                    Sex = response.Sex == "男" ? Sex.男 : Sex.女
                };
            }
            else
            {
            }
        }

        //注册用户
        public async void Re()
        {
            var result = await UserContract.ValidateRegister(UserInfo, "");
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