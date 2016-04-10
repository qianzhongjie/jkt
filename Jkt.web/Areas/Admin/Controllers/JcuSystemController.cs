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
using Bode.Services.Core.Dtos.User;

namespace Bode.Web.Areas.Admin.Controllers
{
    [Description("校区经理管理")]
    public class JcuSystemController : AdminBaseController
    {
        public IStudentContract studentContract { get; set; }
        public IUserContract UserContract { get; set; }
        // GET: Admin/JcuSystem
        [AjaxOnly]
        [Description("获取校区经理信息")]
        public ActionResult GetJcuSystemData(int jcusId)
        {
            int total;
            var student = studentContract.StudentInfos.Where(x => x.ScheduleState > Schedule.申请试练);
            try
            {
                GridRequest request = new GridRequest(Request);
                var datas = GetQueryData<JcuSystem, int>(studentContract.JcuSystems.Where(x => x.Jcus.Id == jcusId), out total,
                        request).Select(m => new
                        {
                            m.CreatedTime,
                            Id = m.Id,
                            HeadPic = m.SystemInfo.HeadPic,
                            NickName = m.SystemInfo.SysUser.NickName,
                            UserName = m.SystemInfo.SysUser.UserName,
                            m.SystemInfo.RealName,
                            SystemInfoId = m.SystemInfo.Id,
                            Name = m.Jcus.Name,
                            m.SystemInfo.Qq,
                            JcusId = m.Jcus.Id,
                            PhoneNumber = m.SystemInfo.SysUser.PhoneNumber,
                            Sum = student.Count(x => x.JcuSystem.Id == m.Id)
                        });
                return Json(new GridData<object>(datas, total), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { });
            }

        }
        //[Description("保存校区经理数据")]
        //[AjaxOnly]
        //[HttpPost]
        //public async Task<ActionResult> SaveJcuSystemData(UserInfoEditDto[] dtos)
        //{
        //    dtos.CheckNotNull("dtos");
        //    OperationResult result = await UserContract.EditUserInfos(dtos: dtos);
        //    return Json(result.ToAjaxResult());
        //}
        [Description("添加校区经理")]
        [HttpPost]
        public async Task<ActionResult> AddJcuSystem(JcuSystemDto[] dtos)
        {
            dtos.CheckNotNull("dtos");
            foreach (var item in dtos)
            {
                var data = studentContract.JcuSystems.Where(x => x.SystemInfo.Id == item.SystemInfoId && x.Jcus.Id == item.JcusId);
                if (data.Any()) return Json(new OperationResult(OperationResultType.QueryNull, "用户已存在").ToAjaxResult());
            }

            OperationResult result = await studentContract.SaveJcuSystems(dtos: dtos);
            return Json(result.ToAjaxResult());
        }

        [HttpPost]
        [AjaxOnly]
        [Description("删除校区经理数据")]
        public async Task<ActionResult> DeleteJcuSystem(int[] ids)
        {
            ids.CheckNotNull("ids");
            foreach (var item in ids)
            {
                var data = studentContract.StudentInfos.Where(x => x.JcuSystem.Id == item && x.IsDeleted == false);
                if (data.Any()) return Json(new OperationResult(OperationResultType.QueryNull, "此管理员下还有学员，不能删除").ToAjaxResult());
            }
            OperationResult result = await studentContract.DeleteJcuSystems(ids);
            return Json(result.ToAjaxResult());
        }
        [Description("校区经理列表")]
        public ActionResult JcuSystemList()
        {
            return View();
        }
        [HttpPost]
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
            }).ToList();
            return Json(new { data = data, datas = datas }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Description("获取系统用户")]
        public ActionResult GetSystemUser()
        {
            var user = UserContract.UserInfos.Where(x => x.SysUser.UserType == UserType.系统用户 && x.SysUser.IsLocked == false).Select(x => new { x.SysUser.UserName, x.Id }).ToList();
            return Json(user);
        }
    }
}