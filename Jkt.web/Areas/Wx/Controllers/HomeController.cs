using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Bode.Common.Json;
using Bode.Services.Core.Contracts;
using Bode.Services.Core.Dtos.User;
using Bode.Services.Core.Dtos.Wx;
using Bode.Services.Core.Models.User;
using WX.Api;
using WX.Model;
using WX.Model.ApiRequests;
using WX.Model.ApiResponses;
using WX.OAuth;
using OSharp.Utility.Logging;
using Bode.Services.Core.Models.Wx;
using OSharp.Utility.Data;

namespace Bode.Web.Areas.Wx.Controllers
{
    [Description("微信dd驾考")]
    public class HomeController : Controller
    {
        public IStudentContract StudentContract { get; set; }
        public IWxContract WxContract { get; set; }
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

        // GET: Wx/Home
        [Description("主页")]
        public ActionResult Index()
        {
            var codeInfo =
                WxContract.TokenCodes.Where(x => x.Type == TokenType.Code).OrderByDescending(x => x.CreatedTime).ToList();
            if (codeInfo.Count() > 0)
            {
                var Code = codeInfo.First().Value;
                if (!string.IsNullOrWhiteSpace(Code))
                {
                    //获取token
                    var ooo = GetToken();
                    var loger = LogManager.GetLogger("Getuser");
                    //        var codeInfos =
                    //WxContract.TokenCodes
                    //    .OrderByDescending(x => x.CreatedTime)
                    //    .ToList();
                    //        var t = codeInfos.Where(x => x.Type == TokenType.Token);
                    //        var o = codeInfos.Where(x => x.Type == TokenType.Other);
                    //        if (t.Count() > 0 && o.Count() > 0)
                    //        {
                    loger.Error("MSG:{0};openId:{1}", "开始判断openidId", ooo);
                    if (!string.IsNullOrWhiteSpace(ooo))
                    {
                        loger.Error("MSG:{0};openId:{1}", "判断成功/开始获取UserInfo", ooo);
                        var userInfo = UserContract.UserInfos.Where(x => x.SysUser.UserName == ooo);
                        if (!userInfo.Any())
                        {
                            loger.Error("MSG:{0};openId:{1}", "到微信获取用户信息", ooo);
                            UserInfoRegistDto dto = GetUserInfo();
                            if (!string.IsNullOrWhiteSpace(dto.NickName))
                            //注册用户
                            {
                                loger.Error("MSG:{0};openId:{1}", "开始注册", ooo);
                                var result = UserContract.Register(dto);
                                loger.Error("MSG:{0};", result.Message);
                                if (result.ResultType == OperationResultType.Success)
                                {
                                    UserInfo.Id = (int)result.Data;
                                }
                            }
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
            }
            ViewBag.NickName = "";
            ViewBag.HeadPic = "";
            ViewBag.OpenId = OpenId;
            if (UserInfo != null)
            {
                ViewBag.NickName = UserInfo.NickName;
                ViewBag.HeadPic = UserInfo.HeadPic;
                ViewBag.OpenId = OpenId;
            }
            //ViewBag.Code = Code;
            //var manager = new OAuthHelper(ConfigurationManager.AppSettings["wxappid"]);
            //var url = manager.BuildOAuthUrl(Host + "Wx/Result/AuthNotifyUrl",
            //     OAuthScope.Base, "getAuth");
            ViewBag.Appid = ConfigurationManager.AppSettings["wxappid"];
            ViewBag.Url = Host + "Wx/Result/AuthNotifyUrl";
            return View();
        }
        [Description("预约试学")]
        public ActionResult GoDate()
        {
            return View();
        }

        [Description("在线咨询")]
        public ActionResult Online()
        {
            return View();
        }

        [Description("学车进度")]
        public ActionResult Progress()
        {
            return View();
        }

        [Description("附近场地")]
        public ActionResult Near()
        {
            return View();
        }

        [Description("当前定位详情")]
        public ActionResult College()
        {
            return View();
        }

        [Description("场地详细")]
        public ActionResult Detail()
        {
            return View();
        }

        [Description("投诉建议")]
        public ActionResult Complaint()
        {
            return View();
        }

        [Description("成功页")]
        public ActionResult SuccessDate()
        {
            return View();
        }

        [Description("okPage")]
        public ActionResult SuccessSubmit()
        {
            return View();
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
        public string GetToken()
        {
            var codeInfo =
                WxContract.TokenCodes.Where(x => x.Type == TokenType.Code)
                    .OrderByDescending(x => x.CreatedTime)
                    .ToList();
            if (codeInfo.Count() > 0)
            {
                var Code = codeInfo.First().Value;
                if (!string.IsNullOrWhiteSpace(Code))
                {
                    var loger = LogManager.GetLogger("RequestToken");
                    var request = new SnsOAuthAccessTokenRequest
                    {
                        AppID = m_appIdent.AppID,
                        AppSecret = m_appIdent.AppSecret,
                        Code = Code
                    };
                    var response = m_client.Execute(request);
                    loger.Error("MSG:{0};Code:{1}", response.ErrorMessage, response.ErrorCode);
                    if (!response.IsError)
                    {
                        //List<TokenCodeDto> dt = new List<TokenCodeDto>();
                        var dto = new TokenCodeDto()
                        {
                            Value = response.AccessToken,
                            Type = TokenType.Token
                        };
                        OpenId = response.OpenId;
                        var dtos = new TokenCodeDto()
                        {
                            Value = response.OpenId,
                            Type = TokenType.Other
                        };
                        //dt.Add(dto);
                        // dt.Add(dtos);
                        WxContract.AddTokenCode(dtos);
                        var re = WxContract.AddTokenCode(dto);
                        return response.OpenId;
                    }
                }
            }
            return "";
        }

        //获取用户信息
        public UserInfoRegistDto GetUserInfo()
        {
            var codeInfo =
            WxContract.TokenCodes
                .OrderByDescending(x => x.CreatedTime)
                .ToList();
            var t = codeInfo.Where(x => x.Type == TokenType.Token);
            //var o = codeInfo.Where(x => x.Type == TokenType.Other);
            if (t.Count() > 0)
            {
                var loger = LogManager.GetLogger("RequestUser");
                SnsUserInfoRequest request = new SnsUserInfoRequest
                {
                    OAuthToken = t.First().Value,
                    Lang = Language.CN,
                    OpenId = OpenId
                };
                SnsUserInfoResponse response = m_client.Execute(request);
                loger.Error("Msg:{0};",response.ErrorMessage );
                if (!response.IsError)
                {
                    loger.Error("json:{0};", JsonTo.ObjectToJsonString<SnsUserInfoResponse>(response));
                    var UserInfoss = new UserInfoRegistDto
                    {
                        Password = response.openId,
                        NickName = response.nickName,
                        UserName = response.openId,
                        Province = response.province,
                        City = response.city,
                        Country = response.country,
                        //RealName = response.NickName,
                        HeadPic = response.headImageUrl,
                        Sex = response.sex == "1" ? Sex.男 : Sex.女
                    };
                    return UserInfoss;
                }
                return new UserInfoRegistDto();
            }
            return new UserInfoRegistDto();
        }

        //注册用户
        public void Re(UserInfoRegistDto dto)
        {
            var loger = LogManager.GetLogger("Register");
            var result = UserContract.Register(dto);
            loger.Error("MSG:{0};", result.Successed);
            if (result.Successed)
            {
                UserInfo.Id = (int)result.Data;
            }
        }
    }
}