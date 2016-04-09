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

namespace Bode.Web.Areas.Admin.Controllers
{
    [Description("学员管理")]
    public class StudentController : AdminBaseController
    {
        public IStudentContract studentContract { get; set; }
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
            var datas = GetQueryData<StudentInfo, int>(studentContract.StudentInfos, out total,
                    request).Select(m => new
                    {
                        m.CreatedTime,
                        m.Id,
                        UserInfoId = m.UserInfo.Id,
                        JcuName = m.Jcu.Name,
                        m.UserRealName,
                        m.ScheduleState,
                        SysRealName = m.JcuSystem.SystemInfo.RealName,
                        m.ExaminationTime
                    });
            return Json(new GridData<object>(datas, total), JsonRequestBehavior.AllowGet);
        }

        [Description("学员列表")]
        public ActionResult StudentList()
        {
            return View();
        }


    }
}