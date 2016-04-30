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
    [Description("学员管理")]
    public class StudentController : AdminBaseController
    {
        public IStudentContract StudentContract { get; set; }
        public IUserContract UserContract { get; set; }
        public IIdentityContract IdentityContract { get; set; }
        // GET: Admin/Student
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [AjaxOnly]
        [Description("获取学员信息")]
        public ActionResult GetStudentData(int jcusId, int userId)
        {
            var user = UserContract.UserInfos.SingleOrDefault(p => p.Id == userId);
            int total;
            GridRequest request = new GridRequest(Request);
            var datas = GetQueryData<StudentInfo, int>(StudentContract.StudentInfos.Where(x => x.Jcu.Id == jcusId), out total,
                    request).Select(m => new
                    {
                        m.CreatedTime,
                        m.Id,
                        UserInfoId = m.UserInfo.Id,
                        JcuName = m.Jcu.Name,
                        m.UserInfo.HeadPic,
                        m.UserRealName,
                        m.UserInfo.SysUser.NickName,
                        m.ScheduleState,
                        m.IdCard,
                        JcuSystemId = m.JcuSystem == null ? 0 : m.JcuSystem.Id,
                        // SysRealName = m.JcuSystem == null ? "" : m.JcuSystem.SystemInfo.RealName,
                        m.ExaminationTime,
                        m.SubscribeTime
                    });
            if (user.SysUser.UserName == "admin" || user.SysUser.UserName == "Admin")
            {

                return Json(new GridData<object>(datas, total), JsonRequestBehavior.AllowGet);
            }
            datas = GetQueryData<StudentInfo, int>(StudentContract.StudentInfos.Where(x => x.Jcu.Id == jcusId && x.JcuSystem.Id == userId), out total,
                    request).Select(m => new
                    {
                        m.CreatedTime,
                        m.Id,
                        UserInfoId = m.UserInfo.Id,
                        JcuName = m.Jcu.Name,
                        m.UserInfo.HeadPic,
                        m.UserRealName,
                        m.UserInfo.SysUser.NickName,
                        m.ScheduleState,
                        m.IdCard,
                        JcuSystemId = m.JcuSystem == null ? 0 : m.JcuSystem.Id,
                        //SysRealName = m.JcuSystem == null ? "" : m.JcuSystem.SystemInfo.RealName,
                        m.ExaminationTime,
                        m.SubscribeTime
                    });
            return Json(new GridData<object>(datas, total), JsonRequestBehavior.AllowGet);
        }
        [AjaxOnly]
        [HttpPost]
        [Description("保存用户数据")]
        public async Task<ActionResult> SaveStudentData(StudentInfoDto[] dtos)
        {
            dtos.CheckNotNull("dtos");
            OperationResult result = await StudentContract.SaveStudentInfos(dtos: dtos);
            return Json(result.ToAjaxResult());
        }

        [Description("获取申请试练数据")]
        [AjaxOnly]
        public ActionResult GetEntryPracticeData()
        {
            int total;
            GridRequest request = new GridRequest(Request);
            var datas = GetQueryData<PracticeEntry, int>(StudentContract.PracticeEntrys, out total,
                    request).Select(m => new
                    {
                        m.Id,
                        //m.UserInfo.SysUser.NickName,
                        m.PhoneNo,
                        m.SubscribeTime,
                        m.Name,
                        m.CreatedTime
                    });
            return Json(new GridData<object>(datas, total), JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpPost]
        [Description("删除试练")]
        public async Task<ActionResult> DeleteEntryPracticeData(int[] ids)
        {
            ids.CheckNotNull("ids");
            OperationResult result = await StudentContract.DeletePracticeEntrys(ids);
            return Json(result.ToAjaxResult());
        }

        [Description("学员列表")]
        public ActionResult StudentList()
        {
            var user = UserContract.UserInfos.SingleOrDefault(p => p.SysUser.UserName == User.Identity.Name);
            ViewBag.UserId = user.Id;
            ViewBag.UserList = StudentContract.JcuSystems.Select(x => new
            {
                val = x.Id,
                text = x.SystemInfo.RealName
            });
            ViewBag.ScheduleState = typeof(Schedule).ToDictionary().Select(p => new
            {
                val = p.Key,
                text = p.Value
            }).ToList();
            return View();
        }

        //[Description("申请试练列表")]
        //public ActionResult EntryPracticeList()
        //{
        //    return View();
        //}


        [HttpPost]
        [Description("获取城市树数据")]
        public ActionResult GetJcuSystemTree(int userId)
        {
            var user = UserContract.UserInfos.SingleOrDefault(p => p.Id == userId);
            var ccc = StudentContract.JcuSystems.Include(x => x.StudentInfo);
            var data = ccc.Where(x => x.SystemInfo.Id == userId).Select(p => new
            {
                value = p.Jcus.City.Id,
                text = p.Jcus.City.Name,
                parentId = 0
            }).ToList();
            var datas = ccc.Where(x => x.SystemInfo.Id == userId).Select(x => new
            {
                value = x.Jcus.Id,
                text = x.Jcus.Name,
                parentId = x.Jcus.City.Id
            }).ToList();
            if (user.SysUser.UserName == "admin" || user.SysUser.UserName == "Admin")
            {
                data = StudentContract.Citys.Include(x => x.Jcu).Select(p => new
                {
                    value = p.Id,
                    text = p.Name,
                    parentId = 0
                }).ToList();
                datas = StudentContract.JCUs.Include(x => x.StudenInfos).Include(x => x.SysUser).Select(x => new
                {
                    value = x.Id,
                    text = x.Name,
                    parentId = x.City.Id
                }).ToList();
                return Json(new { data = data, datas = datas }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = data, datas = datas }, JsonRequestBehavior.AllowGet);
        }
    }
}