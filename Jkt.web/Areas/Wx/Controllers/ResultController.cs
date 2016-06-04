using OSharp.Utility.Logging;
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
using WX.Model;
using WX.OAuth;
using System.Web.Security;
using Bode.Services.Core.Contracts;
using Bode.Services.Core.Dtos.Wx;
using Bode.Services.Core.Models.Wx;

namespace Bode.Web.Areas.Wx.Controllers
{
    [Description("回调控制器")]
    public class ResultController : Controller
    {

        public IWxContract WxContract { get; set; }
        public string Host = ConfigurationManager.AppSettings["ServerHost"];
        [Description("网页授权回调获取code")]
        // GET: Wx/Result
        public ActionResult AuthNotifyUrl(string Code = "")
        {
            if (!string.IsNullOrWhiteSpace(Code))
            {
                var dto = new TokenCodeDto()
                {
                    Value = Code,
                    Type = TokenType.Code
                };
                var loger = LogManager.GetLogger("RequestCode");
                //Code = Request["Code"];
                var result = WxContract.AddTokenCode(dto);
                //打日志
                //loger.Error("orderNo:{0};tradeStatus:{1};", Request["Code"], Request.UrlReferrer);
                return RedirectToAction("Index", "Home", new { count = 1 });
            }
            return Content("code");
            //return Content(Code);
        }

        [Description("引导授权")]
        public ActionResult AuthTokenUrl()
        {
            var manager = new OAuthHelper(ConfigurationManager.AppSettings["wxappid"]);
            var url = manager.BuildOAuthUrl(Host + "Wx/Result/AuthNotifyUrl",
                 OAuthScope.Base, "getAuth");

            //发起请求
            // m_client.Execute(url);
            //WebRequest myWebRequest = WebRequest.Create(url);

            //WebResponse myWebResponse = myWebRequest.GetResponse();
            //Stream ReceiveStream = myWebResponse.GetResponseStream();
            //string responseStr = "";
            //if (ReceiveStream != null)
            //{
            //    StreamReader reader = new StreamReader(ReceiveStream, Encoding.UTF8);
            //    responseStr = reader.ReadToEnd();
            //    reader.Close();
            //}
            //myWebResponse.Close();
            ViewBag.Body = url;
            return View();
        }

    }
}