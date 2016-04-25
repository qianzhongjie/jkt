using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bode.Web.Areas.Wx.Controllers
{
    [Description("微信dd驾考")]
    public class HomeController : Controller
    {
        // GET: Wx/Home
        [Description("主页")]
        public ActionResult Index()
        {
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
    }
}