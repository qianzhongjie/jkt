using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bode.Services.Core.Contracts;
using Bode.Services.Core.Dtos.Bnaners;
using Bode.Services.Core.Models.Student;
using OSharp.Web.Mvc;
using OSharp.Web.Mvc.Security;
using OSharp.Web.Mvc.UI;
using Bode.Services.Core.Models.Bnaners;
using OSharp.Utility;
using OSharp.Utility.Data;

namespace Bode.Web.Areas.Admin.Controllers
{
    [Description("Banner管理")]
    public class BannerController : AdminBaseController
    {
        public IBnanersContract BnanersContract { get; set; }
        // GET: Admin/Banner
        [AjaxOnly]
        [Description("获取广告信息")]
        public ActionResult GetBannerData()
        {
            int total;
            GridRequest request = new GridRequest(Request);
            var datas = GetQueryData<Bnaner, int>(BnanersContract.Bnaners, out total,
                    request).Select(m => new
                    {
                        m.Id,
                        m.Path,
                        m.Url,
                        m.IsDisPlay,

                        m.CreatedTime
                    });
            return Json(new GridData<object>(datas, total), JsonRequestBehavior.AllowGet);
        }
        [AjaxOnly]
        [HttpPost]
        [Description("保存广告数据")]
        public async Task<ActionResult> SaveBannerData(BnanerDto[] dtos)
        {
            dtos.CheckNotNull("dtos");
            OperationResult result = await BnanersContract.SaveBnaners(dtos: dtos);
            return Json(result.ToAjaxResult());
        }
        [AjaxOnly]
        [HttpPost]
        [Description("删除广告数据")]
        public async Task<ActionResult> DeleteBnaners(int[] ids)
        {
            ids.CheckNotNull("ids");
            OperationResult result = await BnanersContract.DeleteBnaners(ids);
            return Json(result.ToAjaxResult());
        }

        [Description("主页Banner")]
        public ActionResult BnnerList()
        {
            return View();
        }
    }
}