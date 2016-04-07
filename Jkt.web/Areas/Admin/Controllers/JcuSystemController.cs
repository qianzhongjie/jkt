using Bode.Services.Core.Contracts;
using Bode.Services.Core.Models.Student;
using OSharp.Web.Mvc;
using OSharp.Web.Mvc.Security;
using OSharp.Web.Mvc.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Bode.Services.Core.Dtos.Student;
using System.Threading.Tasks;
using OSharp.Utility;
using OSharp.Utility.Data;
using Bode.Services.Core.Models.Identity;

namespace Bode.Web.Areas.Admin.Controllers
{
    [Description("校区经理管理")]
    public class JcuSystemController : AdminBaseController
    {
        public IStudentContract studentContract { get; set; }
        public IdentityController dentityController { get; set; }
        // GET: Admin/JcuSystem
        [AjaxOnly]
        [Description("获取校区经理信息")]
        public ActionResult GetJcuSystemData(int jcusId)
        {
            int total;
            var student = studentContract.StudentInfos.Where(x => x.ScheduleState > Schedule.未试练);
            GridRequest request = new GridRequest(Request);
            var datas = GetQueryData<JcuSystem, int>(studentContract.JcuSystems.Where(x => x.Jcus.Id == jcusId), out total,
                    request).Select(m => new
                    {
                        m.CreatedTime,
                        m.Id,
                        m.SystemInfo.RealName,
                        m.SystemInfo.SysUser.NickName,
                        SystemInfoId = m.SystemInfo.Id,
                        m.Jcus.Name,
                        JcusId = m.Jcus.Id,
                        m.SystemInfo.SysUser.PhoneNumber,
                        Sum = student.Count(x => x.JcuSystem.Id == m.Id)
                    });
            return Json(new GridData<object>(datas, total), JsonRequestBehavior.AllowGet);
        }
        [Description("保存校区经理数据")]
        public async Task<ActionResult> SaveJcuSystemData(JcuSystemDto[] dtos)
        {
            dtos.CheckNotNull("dtos");
            OperationResult result = await studentContract.SaveJcuSystems(dtos: dtos);
            return Json(result.ToAjaxResult());
        }


        [HttpPost]
        [Description("删除校区经理数据")]
        public async Task<ActionResult> DeleteJcuSystem(int[] ids)
        {
            ids.CheckNotNull("ids");
            OperationResult result = await Task.Run(() => studentContract.DeleteJcuSystems(ids));
            return Json(result.ToAjaxResult());
        }
        [Description("校区经理列表")]
        public ActionResult JcuSystemList()
        {
            return View();
        }
        [AjaxOnly]
        [Description("获取城市树数据")]
        public ActionResult GetJcuSystemTree()
        {
            var data = studentContract.Citys.Include(x => x.Jcu).Select(p => new
            {
                value = p.Id,
                text = p.Name,
                parentId = 0
            }).ToList();
            var datas = studentContract.JCUs.Include(x => x.StudenInfos).Include(x => x.SysUser).Select(x => new
            {
                value = x.Id,
                text = x.Name,
                parentId = x.City.Id
            });
            data.AddRange(datas);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Description("获取系统用户")]
        public ActionResult GetSystemUser()
        {
            var user = dentityController.UserContract.UserInfos.Where(x => x.SysUser.UserType == UserType.系统用户 && x.SysUser.IsLocked == false).Select(x => new { x.SysUser.NickName, x.Id });
            return Json(user.ToList());
        }
    }
}