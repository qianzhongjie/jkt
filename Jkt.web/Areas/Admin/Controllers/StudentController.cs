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
        public IStudentContract studentContract { get; set; }
        public IUserContract UserContract { get; set; }
        public IIdentityContract IdentityContract { get; set; }
        // GET: Admin/Student
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [AjaxOnly]
        [Description("获取学员信息")]
        public ActionResult GetStudentData(int jcusId)
        {
            int total;
            GridRequest request = new GridRequest(Request);
            var datas = GetQueryData<StudentInfo, int>(studentContract.StudentInfos.Where(x => x.Jcu.Id == jcusId), out total,
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
                        SysRealName = m.JcuSystem.SystemInfo.RealName,
                        m.ExaminationTime
                    });
            return Json(new GridData<object>(datas, total), JsonRequestBehavior.AllowGet);
        }
        [AjaxOnly]
        [HttpPost]
        [Description("保存用户数据")]
        public async Task<ActionResult> SaveStudentData(StudentInfoDto[] dtos)
        {
            dtos.CheckNotNull("dtos");
            OperationResult result = await studentContract.SaveStudentInfos(dtos: dtos);
            return Json(result.ToAjaxResult());
        }
        [Description("学员列表")]
        public ActionResult StudentList()
        {
            var user = UserContract.UserInfos.SingleOrDefault(p => p.SysUser.UserName == User.Identity.Name);
            ViewBag.UserId = user.Id;
            ViewBag.ScheduleState = typeof(Schedule).ToDictionary().Where(x => x.Key != (int)Schedule.未试练).Select(p => new
            {
                val = p.Key,
                text = p.Value
            }).ToList();
            return View();
        }

        [Description("申请试练列表")]
        public ActionResult EntryPracticeList()
        {
            return View();
        }


        [HttpPost]
        [Description("获取城市树数据")]
        public ActionResult GetJcuSystemTree(int userId)
        {
            var user = UserContract.UserInfos.SingleOrDefault(p => p.Id == userId);
            var ccc = studentContract.JcuSystems.Include(x => x.StudentInfo);
            var data = ccc.Select(p => new
            {
                value = p.Jcus.City.Id,
                text = p.Jcus.City.Name,
                parentId = 0
            }).ToList();
            var datas = ccc.Select(x => new
            {
                value = x.Jcus.Id,
                text = x.Jcus.Name,
                parentId = x.Jcus.City.Id
            });
            if (user.SysUser.UserName == "admin" || user.SysUser.UserName == "Admin")
            {
            }
            else {
                data = ccc.Where(x => x.SystemInfo.Id == userId).Select(p => new
                {
                    value = p.Jcus.City.Id,
                    text = p.Jcus.City.Name,
                    parentId = 0
                }).ToList();
                datas = ccc.Where(x => x.SystemInfo.Id == userId).Select(x => new
                {
                    value = x.Jcus.Id,
                    text = x.Jcus.Name,
                    parentId = x.Jcus.City.Id
                });
            }
            return Json(new { data = data, datas = datas }, JsonRequestBehavior.AllowGet);
        }
    }
}