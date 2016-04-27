using OSharp.Utility.Logging;
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
using Bode.Common.Data;
using WX.Model;
using WX.OAuth;

namespace Bode.Web.Areas.Wx.Controllers
{
    [Description("回调控制器")]
    public class ResultController : FristController
    {
        public string Host = ConfigurationManager.AppSettings["ServerHost"];
        [Description("网页授权回调获取code")]
        // GET: Wx/Result
        public ActionResult AuthNotifyUrl()
        {
            var loger = LogManager.GetLogger("RequestCode");
            Code = Request["Code"];
            //打日志
            loger.Error("orderNo:{0};tradeStatus:{1};", Request["Code"], Request.UrlReferrer);
            return Content(Code);
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