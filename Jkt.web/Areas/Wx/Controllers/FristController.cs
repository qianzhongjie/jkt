using OSharp.Utility.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bode.Web.Areas.Wx.Controllers
{
    [Description("控制器父类")]
    public class FristController : Controller
    {
        //public string Code { get; set; }

        public FristController()
        {
            //AuthNotifyUrl();
        }

        //[Description("网页授权回调获取/Code")]
        //// GET: Wx/Result
        //public ActionResult AuthNotifyUrl()
        //{
        //    var loger = LogManager.GetLogger("RequestCode");
        //    Code = Request["Code"] ?? "";
        //    //打日志
        //    loger.Error("orderNo:{0};tradeStatus:{1};", Request["Code"], Request.UrlReferrer);
        //    return RedirectToAction("Index", "Home");
        //    //return Content(Code);
        //}
    }
}