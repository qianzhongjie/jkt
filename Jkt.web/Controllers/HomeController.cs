using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bode.Services.Core.Contracts;
using Bode.Services.Core.Dtos.User;
using Bode.Services.Core.Models.User;
using System.Threading;

namespace Bode.Web.Controllers
{
    [Description("系统主页")]
    public class HomeController : Controller
    {
        public IUserContract UserContract { get; set; }

        // GET: Home

        [Description("主页")]
        public ActionResult Index()
        {
            //string modelFile = Path.Combine(projectPath, @"bin\Debug\Bode.Services.Core.dll");
            //byte[] fileData = File.ReadAllBytes(modelFile);
            //Assembly assembly = Assembly.Load("Bode.Services.Core");
            //Type baseType = typeof(EntityBase<>);
            //IEnumerable<Type> modelTypes = assembly.GetTypes().Where(m => baseType.IsGenericAssignableFrom(m) && !m.IsAbstract && m.HasAttribute<GenerateAttribute>());

            //ValidateCodeDto validate = new ValidateCodeDto()
            //{
            //    CodeKey = "18628303252",
            //    Code = "123456",
            //    CodeType = CodeType.用户注册
            //};

            //await UserContract.SaveValidateCodes(dtos: validate);
            //int threadId1 = Thread.CurrentThread.ManagedThreadId;

            //var codes = await UserContract.ValidateCodes.ToListAsync().ConfigureAwait(false);

            //int threadId2 = Thread.CurrentThread.ManagedThreadId;

            //string content = string.Format("start:{0};end:{1};", threadId1, threadId2);
            return Redirect("/Admin/Home/Index");
        }

        [Description("微信Url授权")]
        public ActionResult Notifly_url()
        {
            string token = "didijiakao";
            if (string.IsNullOrEmpty(token))
            {
                return Content("");
            }

            string echoString = HttpContext.Request.QueryString["echoStr"];
            string signature = HttpContext.Request.QueryString["signature"];
            string timestamp = HttpContext.Request.QueryString["timestamp"];
            string nonce = HttpContext.Request.QueryString["nonce"];

            if (!string.IsNullOrEmpty(echoString))
            {
                HttpContext.Response.Write(echoString);
                HttpContext.Response.End();
            }
            return Content(echoString);
        }
        [Description("验证并注册")]
        [HttpPost]
        public async Task<ActionResult> ValidateRegister(UserInfoRegistDto dto, string validateCode)
        {
            var result = await UserContract.ValidateRegister(dto, validateCode);
            return Json(result);
        }

    }
}