using Bode.Services.Core.Contracts;
using Bode.Services.Core.Dtos.Student;
using Bode.Services.Core.Models.Student;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Extensions;
using OSharp.Web.Mvc;
using OSharp.Web.Mvc.Security;
using OSharp.Web.Mvc.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Bode.Web.Areas.Admin.Controllers
{
    [Description("场地管理")]
    public class SiteFactoryController : AdminBaseController
    {

        public IUserContract UserContract { get; set; }
        public IStudentContract StudentContract { get; set; }

        [AjaxOnly]
        [Description("获取场地信息")]
        public ActionResult GetSiteFactoryData(int jcusId)
        {
            int total;
            GridRequest request = new GridRequest(Request);
            var datas = GetQueryData<SiteFactory, int>(StudentContract.SiteFactorys.Where(x => x.JCU.Id == jcusId),
                out total,
                request).Select(m => new
                {
                    m.Id,
                    m.Address,
                    m.Cover,
                    m.CreatedTime,
                    m.Lat,
                    m.Lng,
                    m.Name,
                });
            return Json(new GridData<object>(datas, total), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Description("保存场地数据")]
        public async Task<ActionResult> SaveSiteFactoryData(SiteFactoryDto dtos, SitePic[] pic)
        {
            dtos.CheckNotNull("dtos");
            var model = StudentContract.SiteFactorys.Where(x => x.Name == dtos.Name);
            if (model.Any())
            {
                if (dtos.Id == 0)
                {
                    return Json(new OperationResult(OperationResultType.QueryNull, "此名称已存在,请不要重复添加").ToAjaxResult());
                }
                else if (dtos.Id != model.First().Id)
                {
                    return Json(new OperationResult(OperationResultType.QueryNull, "此名称已存在,请不要重复添加").ToAjaxResult());
                }
            }
            OperationResult result = await StudentContract.AddFactory(dtos, pic);
            return Json(result.ToAjaxResult());
        }

        [AjaxOnly]
        [HttpPost]
        [Description("删除场地数据")]
        public async Task<ActionResult> DeleteSiteFactoryData(int[] ids)
        {
            ids.CheckNotNull("ids");
            OperationResult result = await StudentContract.DeleteSiteFactorys(ids);
            return Json(result.ToAjaxResult());
        }

        [Description("场地列表")]
        // GET: Admin/SiteFactory
        public ActionResult SiteFatoryList()
        {
            var user = UserContract.UserInfos.SingleOrDefault(p => p.SysUser.UserName == User.Identity.Name);
            ViewBag.UserId = user.Id;
            ViewBag.UserName = user.SysUser.UserName;
            return View();
        }

        [Description("添加场地")]
        public ActionResult EditAddSiteFatory(int stylistId)
        {
            ViewBag.Id = stylistId;
            return View();
        }

        [Description("编辑场地")]
        public ActionResult EditSiteFatory(int id)
        {
            ViewBag.Data = StudentContract.SiteFactorys.Single(x => x.Id == id);
            return View();
        }
    }
}