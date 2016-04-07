using Bode.Services.Core.Contracts;
using Bode.Services.Core.Dtos.Student;
using Bode.Services.Core.Models.Student;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Web.Mvc;
using OSharp.Web.Mvc.Security;
using OSharp.Web.Mvc.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Bode.Web.Areas.Admin.Controllers
{


    [Description("城市管理")]
    public class CityController : AdminBaseController
    {
        public IStudentContract studentContract { get; set; }
        [AjaxOnly]
        [Description("获取城市信息")]
        public ActionResult GetCityData()
        {
            int total;
            GridRequest request = new GridRequest(Request);
            var datas = GetQueryData<City, int>(studentContract.Citys, out total,
                    request).Select(m => new
                    {
                        m.Id,
                        m.Name,
                        m.ProName,
                        m.CreatedTime
                    });
            return Json(new GridData<object>(datas, total), JsonRequestBehavior.AllowGet);
        }
        [AjaxOnly]
        [HttpPost]
        [Description("保存城市数据")]
        public async Task<ActionResult> SaveCityData(CityDto[] dtos)
        {
            dtos.CheckNotNull("dtos");
            OperationResult result = await studentContract.SaveCitys(dtos: dtos);
            return Json(result.ToAjaxResult());
        }
        [AjaxOnly]
        [HttpPost]
        [Description("删除城市数据")]
        public async Task<ActionResult> DeleteCity(int[] ids)
        {
            ids.CheckNotNull("ids");
            OperationResult result = await Task.Run(() => studentContract.DeleteCitys(ids));
            return Json(result.ToAjaxResult());
        }
        // GET: Admin/City
        [Description("城市列表")]
        public ActionResult CityList()
        {
            return View();
        }
    }
}