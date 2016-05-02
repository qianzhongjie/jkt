using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bode.Services.Core.Contracts;
using Bode.Services.Core.Dtos.Config;
using Bode.Services.Core.Dtos.Student;
using Bode.Services.Core.Models.Config;
using Bode.Services.Core.Models.Student;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Extensions;
using OSharp.Web.Mvc;
using OSharp.Web.Mvc.Security;
using OSharp.Web.Mvc.UI;

namespace Bode.Web.Areas.Admin.Controllers
{
    [Description("配置管理")]
    public class ConfigController : AdminBaseController
    {
        public IConfigContract ConfigContract { get; set; }

        [AjaxOnly]
        [Description("获取配置信息")]
        public ActionResult GetConfigData()
        {
            int total;
            GridRequest request = new GridRequest(Request);
            var datas = GetQueryData<ContactConfig, int>(ConfigContract.ContactConfigs, out total,
                    request).Select(m => new
                    {
                        m.Id,
                        m.Value,
                        m.Type,
                        m.CreatedTime
                    });
            return Json(new GridData<object>(datas, total), JsonRequestBehavior.AllowGet);
        }
        [AjaxOnly]
        [HttpPost]
        [Description("保存配置数据")]
        public async Task<ActionResult> SaveConfigData(ContactConfigDto[] dtos)
        {
            dtos.CheckNotNull("dtos");
            OperationResult result = await ConfigContract.SaveContactConfigs(dtos: dtos);
            return Json(result.ToAjaxResult());
        }

        [AjaxOnly]
        [HttpPost]
        [Description("删除配置数据")]
        public async Task<ActionResult> DeleteConfig(int[] ids)
        {
            ids.CheckNotNull("ids");
            OperationResult result = await Task.Run(() => ConfigContract.DeleteContactConfigs(ids));
            return Json(result.ToAjaxResult());
        }

        [Description("配置联系方式")]
        // GET: Admin/Config
        public ActionResult ConfigList()
        {
            ViewBag.Type = typeof(ContactType).ToDictionary().Where(x =>x.Key != 2).Select(p => new
            {
                val = p.Key,
                text = p.Value
            }).ToList();
            return View();
        }
    }
}