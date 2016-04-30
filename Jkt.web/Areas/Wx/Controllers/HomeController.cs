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
using Bode.Common.Data;
using Bode.Common.Json;
using Bode.Common.Result;
using Bode.Services.Core.Contracts;
using Bode.Services.Core.Dtos.Student;
using Bode.Services.Core.Dtos.User;
using Bode.Services.Core.Dtos.Wx;
using Bode.Services.Core.Models.Student;
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
using WeChatJsSdk.SdkCore;
using OSharp.Web.Http.Messages;

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
        public ActionResult Index(int count = 0)
        {
            //count++;
            ViewBag.Count = count;
            if (count > 0)
            {
                var codeInfo =
                    WxContract.TokenCodes.Where(x => x.Type == TokenType.Code)
                        .OrderByDescending(x => x.CreatedTime)
                        .ToList();

                if (codeInfo.Count() > 0)
                {
                    var code = codeInfo.First().Value;
                    if (!string.IsNullOrWhiteSpace(code))
                    {
                        //获取token
                        var ooo = GetTokenOrOpenId(code);
                        var loger = LogManager.GetLogger("Getuser");
                        //loger.Error("MSG:{0};openId:{1}", "开始判断openidId", ooo);
                        if (!string.IsNullOrWhiteSpace(ooo))
                        {
                            //loger.Error("MSG:{0};openId:{1}", "判断成功/开始获取UserInfo", ooo);
                            try
                            {
                                UserInfoRegistDto dto = GetUserInfo(ooo);
                                var userInfo =
                                    UserContract.UserInfos.Where(x => x.SysUser.UserName == ooo)
                                        .Select(x => new UserInfoRegistDto()
                                        {
                                            Id = x.Id,
                                            UserName = x.SysUser.UserName,
                                            NickName = x.SysUser.NickName,
                                            //Password = x.SysUser.PasswordHash,
                                            //Token = x.Token,
                                            HeadPic = x.HeadPic,
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
                                        loger.Error("MSG:{0};openId:{1}", "开始注册", ooo);
                                        var result = UserContract.Register(dto);
                                        loger.Error("MSG:{0};", result.Message);
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
                                    UserId = dto.Id;
                                    ViewBag.NickName = userInfo.First().NickName;
                                    ViewBag.HeadPic = userInfo.First().HeadPic;
                                    ViewBag.OpenId = userInfo.First().UserName;
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
            }
            ViewBag.Appid = ConfigurationManager.AppSettings["wxappid"];
            ViewBag.Url = Host + "Wx/Result/AuthNotifyUrl";
            return View();
        }
        #region ajax

        [Description("提交试练")]
        [HttpPost]
        public async Task<ActionResult> SaveGodata(string openId, string name, string phone, DateTime time, double lng = 0, double lat = 0)
        {
            if (lng == 0)
            {
                return Json(new ApiResult(OperationResultType.ValidError, "无法获取你的定位,请打开定位,再提交"));
            }
            if (lat == 0)
            {
                return Json(new ApiResult(OperationResultType.ValidError, "无法获取你的定位,请打开定位,再提交"));
            }
            var jcuList = StudentContract.JCUs.AsEnumerable().Select(x => new
            {
                x.Id,
                x.Lat,
                x.Log,
                Distance = LngLat.GetDistance(Double.Parse(x.Lat), Double.Parse(x.Log), lat, lng)
            }).OrderBy(x => x.Distance);
            if (jcuList.Any())
            {
                var jcu = jcuList.First();
                var JcuSys = StudentContract.JcuSystems.Where(x => x.Jcus.Id == jcu.Id);
                var userinfo = UserContract.UserInfos.Where(x => x.Token == openId);
                var userId = 0;
                if (userinfo.Any())
                {
                    userId = userinfo.First().Id;
                }
                else
                {
                    return Json(new ApiResult(OperationResultType.ValidError, "服务器繁忙,请稍后再试"));
                }
                var dto = new StudentInfoDto()
                {
                    UserInfoId = userId,
                    ScheduleState = Schedule.申请试练,
                    JcuId = jcu.Id,
                    JcuSystemId = JcuSys.Any() ? JcuSys.First().Id : 0,
                    Lat = lat.ToString(),
                    Lng = lng.ToString(),
                    UserRealName = name,
                    IdCard = phone
                };
                var result = await StudentContract.AddSutdent(dto, time);
                return Json(result.ToApiResult());
            }
            return Json(new ApiResult(OperationResultType.ValidError, "对不起,目前未开放你所在地区,敬请期待"));
        }

        #endregion
        #region 视图
        [Description("预约试学")]
        public ActionResult GoDate(string openId)
        {
            ViewBag.OpenId = openId;
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
        //获取网页授权token和openid
        public string GetTokenOrOpenId(string code)
        {
            //var codeInfo =
            //    WxContract.TokenCodes.Where(x => x.Type == TokenType.Code)
            //        .OrderByDescending(x => x.CreatedTime);
            //if (codeInfo.Count() > 0)
            //{
            //var code = codeInfo.First().Value;
            //if (!string.IsNullOrWhiteSpace(code))
            //{
            var loger = LogManager.GetLogger("RequestToken");
            var response = WxResult.GetOpenId(code);
            loger.Error("xxxxx:{0}", JsonTo.ObjectToJsonString(response));
            loger.Error("MSG:{0};Code:{1}", response.errcode, response.errmsg);
            loger.Error("openid:{0}", response.openid);
            return response.openid;
            // }
            //}
            //return "";
        }

        //获取用户信息
        public UserInfoRegistDto GetUserInfo(string openid)
        {
            var loger = LogManager.GetLogger("RequestUser");
            var token = GetTokenDb();
            loger.Error("newToken:{0};", token);
            if (!string.IsNullOrWhiteSpace(token))
            {
                var response = WxResult.GetUserInfo(token, openid);
                loger.Error("msg:{0};", JsonTo.ObjectToJsonString(response));
                loger.Error("msg:{0};", "用户信息返回成功" + response.openId);
                var UserInfoss = new UserInfoRegistDto
                {
                    Password = response.openId,
                    Token = response.openId,
                    NickName = response.nickName,
                    UserName = response.openId,
                    Province = response.province,
                    City = response.city,
                    Country = response.country,
                    HeadPic = response.headimgurl,
                    Sex = response.sex == "1" ? Sex.男 : Sex.女
                };
                return UserInfoss;
            }
            return new UserInfoRegistDto() { };
        }

        /// <summary>
        /// 获取基础token判断数据库token是否可用
        /// </summary>
        /// <returns></returns>
        public string GetTokenDb()
        {
            var loger = LogManager.GetLogger("RequestToken");
            var t = WxContract.TokenCodes.Where(x => x.Type == TokenType.Token).OrderByDescending(x => x.CreatedTime).ToList();
            if (t.Count() > 0)
            {
                var time = t.First().CreatedTime;
                if (time.AddSeconds(7200) > DateTime.Now)
                {
                    loger.Error("msg:{0};", "获取token成功/Db", t.First().Value);
                    return t.First().Value;
                }
            }
            var token = WxResult.GetToken();
            var dto = new TokenCodeDto()
            {
                Value = token.access_token,
                Type = TokenType.Token
            };
            if (string.IsNullOrWhiteSpace(token.access_token))
            {
                loger.Error("msg:{0};", "获取token失败");
            }
            var result = WxContract.AddTokenCode(dto);
            loger.Error("msg:{0};", "获取token成功", token.access_token);
            return dto.Value;
        }

        /// <summary>
        /// 获取可用tickt
        /// </summary>
        /// <returns></returns>
        public string GetTicketDb()
        {
            var loger = LogManager.GetLogger("RequestTicket");
            var t = WxContract.TokenCodes.Where(x => x.Type == TokenType.Other).OrderByDescending(x => x.CreatedTime).ToList();
            if (t.Count() > 0)
            {
                var time = t.First().CreatedTime;
                if (time.AddSeconds(7200) > DateTime.Now)
                {
                    loger.Error("msg:{0};", "获取ticket成功/Db", t.First().Value);
                    return t.First().Value;
                }
            }
            var token = WxResult.GetTicket(GetTokenDb());
            var dto = new TokenCodeDto()
            {
                Value = token.ticket,
                Type = TokenType.Other
            };
            if (string.IsNullOrWhiteSpace(token.ticket))
            {
                loger.Error("msg:{0};", "获取ticket失败");
            }
            var result = WxContract.AddTokenCode(dto);
            loger.Error("msg:{0};", "获取ticket成功", token.ticket);
            return dto.Value;
        }

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <returns></returns>
        [Description("获取js配置")]
        [HttpPost]
        public ActionResult JsConfig(string urlNow)
        {
            string noncestr = WxResult.GetString(10);
            string jsapi_ticket = GetTicketDb();
            string timestamp = WxResult.GetTimeStamp();
            //string url = Request.Url.AbsoluteUri.ToString();
            Dictionary<string, string> signData = new Dictionary<string, string>() {
                {"jsapi_ticket",jsapi_ticket},
                {"noncestr",noncestr},
                {"timestamp",timestamp.ToString()},
                {"url",urlNow}
            };
            Signature st = new Signature();
            string mySign = st.Sign(signData);
            // string str = "jsapi_ticket=" + jsapi_ticket + "&noncestr=" + noncestr + "&timestamp=" + timestamp + "&url =" + urlNow;
            // var sing = DataCode.Sha1(str);
            return Json(new
            {
                appId = ConfigurationManager.AppSettings["wxappid"],
                timestamp = timestamp,
                noncestr = noncestr,
                signature = mySign,
            });
        }
    }
}
#endregion