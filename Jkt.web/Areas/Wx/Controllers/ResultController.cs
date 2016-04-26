using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bode.Web.Areas.Wx.Controllers
{
    [Description("回调控制器")]
    public class ResultController : WxController
    {
        [Description("网页授权回调获取code")]
        // GET: Wx/Result
        public ActionResult AuthNotifyUrl()
        {
            Code = Request["Code"];
            return Content(Code);
        }
    }
}