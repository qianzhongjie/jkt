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
using Bode.Common.Json;
using Bode.Common.Result;
using Bode.Services.Core.Contracts;
using Bode.Services.Core.Dtos.Student;
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
using WX;
using WX.Framework;

namespace Bode.Web.Areas.Wx.Controllers
{
    [Description("微信dd驾考")]
    public class HomeController : Controller
    {
        public IStudentContract StudentContract { get; set; }
        public IWxContract WxContract { get; set; }
        public static string OpenId { get; set; }
        public static int UserId { get; set; }
        public string Host = ConfigurationManager.AppSettings["ServerHost"];
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
                    //loger.Error("MSG:{0};openId:{1}", "开始判断openidId", ooo);
                    if (!string.IsNullOrWhiteSpace(ooo))
                    {
                        //loger.Error("MSG:{0};openId:{1}", "判断成功/开始获取UserInfo", ooo);
                        try
                        {
                            UserInfoRegistDto dto = GetUserInfo();
                            var userInfo = UserContract.UserInfos.Where(x => x.SysUser.UserName == ooo).Select(x => new UserInfoRegistDto()
                            {
                                Id = x.Id
                                //UserName = x.SysUser.UserName,
                                //NickName = x.SysUser.NickName,
                                //Password = x.SysUser.PasswordHash,
                                //Token = x.Token,
                                //HeadPic = x.HeadPic,
                                //Sex = x.Sex,
                                //Province = x.Province,
                                //City = x.City,
                                //Country = x.City,
                            }).ToList();
                            if (userInfo.Count == 0)
                            {
                                // loger.Error("MSG:{0};openId:{1}", "到微信获取用户信息", ooo);
                                if (!string.IsNullOrWhiteSpace(dto.NickName))
                                //注册用户
                                {
                                    //loger.Error("MSG:{0};openId:{1}", "开始注册", ooo);
                                    var result = UserContract.Register(dto);
                                    // loger.Error("MSG:{0};", result.Message);
                                    var id = (int)result.Data;
                                    var u = UserContract.UserInfos.Single(x => x.Id == id);
                                    ViewBag.NickName = u.SysUser.NickName;
                                    ViewBag.HeadPic = u.HeadPic;
                                    ViewBag.OpenId = u.SysUser.UserName;
                                    UserId = u.Id;
                                }
                            }
                            else
                            {
                                dto.Id = userInfo.First().Id;
                                UserContract.Register(dto);
                                //跟新userId
                                UserId = dto.Id;
                                // UserInfo.UserName = user.SysUser.UserName;
                                //UserInfo.NickName = user.SysUser.NickName;
                                //UserInfo.RealName = user.RealName;
                                //UserInfo.HeadPic = user.HeadPic;
                                //UserInfo.Sex = userOne.Sex;
                                ViewBag.NickName = dto.NickName;
                                ViewBag.HeadPic = dto.HeadPic;
                                ViewBag.OpenId = dto.UserName;
                            }
                        }
                        catch (Exception e)
                        {
                            ViewBag.OpenId = "xx";
                            loger.Error("MSG:{0};", e.Message);
                        }
                    }
                }
            }
            ViewBag.Appid = ConfigurationManager.AppSettings["wxappid"];
            ViewBag.Url = Host + "Wx/Result/AuthNotifyUrl";
            return View();
        }
        #region ajax

        [Description("提交试练")]
        [HttpPost]
        public async Task<ActionResult> SaveGodata(string name, string phone, string time)
        {
            var result = await StudentContract.SaveStudentInfos(dtos: new StudentInfoDto
            {

            });
            return Json(new { });
        }

        #endregion
        # region 视图
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
            var progress = StudentContract.StudentInfos.Single(x => x.UserInfo.Id == UserId);
            if (progress != null)
            {
                ViewBag.progress = progress.ScheduleState;
                return View();
            }
            ViewBag.progress = 8;
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
        #endregion
        #region 私有
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
                    //var loger = LogManager.GetLogger("RequestToken");
                    var request = new SnsOAuthAccessTokenRequest
                    {
                        AppID = m_appIdent.AppID,
                        AppSecret = m_appIdent.AppSecret,
                        Code = Code
                    };
                    var response = m_client.Execute(request);
                    // loger.Error("MSG:{0};Code:{1}", response.ErrorMessage, response.ErrorCode);
                    if (!response.IsError)
                    {
                        //List<TokenCodeDto> dt = new List<TokenCodeDto>();
                        //var dto = new TokenCodeDto()
                        //{
                        //    Value = response.AccessToken,
                        //    Type = TokenType.Token
                        //};
                        OpenId = response.OpenId;
                        //var dtos = new TokenCodeDto()
                        //{
                        //    Value = response.OpenId,
                        //    Type = TokenType.Other
                        //};
                        //dt.Add(dto);
                        // dt.Add(dtos);
                        //WxContract.AddTokenCode(dtos);
                        //var re = WxContract.AddTokenCode(dto);
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
                // var loger = LogManager.GetLogger("RequestUser");
                var token = WxResult.GetToken();
                // loger.Error("newToken:{0};", token.access_token);
                if (!string.IsNullOrWhiteSpace(token.access_token))
                {
                    var response = WxResult.GetUserInfo(token.access_token, OpenId);
                    //loger.Error("Msg:{0};", response.errmsg + response.errcode);
                    if (!string.IsNullOrWhiteSpace(response.openId))
                    {
                        //loger.Error("msg:{0};", "用户信息返回成功" + response.openId);
                        var UserInfoss = new UserInfoRegistDto
                        {
                            Password = response.openId,
                            Token = response.openId,
                            NickName = response.nickName,
                            UserName = response.openId,
                            Province = response.province,
                            City = response.city,
                            Country = response.country,
                            //RealName = response.NickName,
                            HeadPic = response.headimgurl,
                            Sex = response.sex == "1" ? Sex.男 : Sex.女
                        };
                        return UserInfoss;
                    }
                }

                return new UserInfoRegistDto();
            }
            return new UserInfoRegistDto();
        }
    }
}
        #endregion