using Bode.Services.Core.Models.User;
using OSharp.Core.Data;
using OSharp.Utility.Develop.T4;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bode.Services.Core.Models.Student
{
    [Generate]
    [Description("学员实体")]
    public class StudentInfo : EntityBase<int>
    {
        [Description("关联的用户信息")]
        public virtual UserInfo UserInfo { get; set; }

        [Description("进度")]
        public Schedule ScheduleState { get; set; }

        [Description("所属校区")]
        public virtual JCU Jcu { get; set; }

        [Description("所属经理")]
        public virtual JcuSystem JcuSystem { get; set; }

        //[Description("预约考试时间")]
        public DateTime? ExaminationTime { get; set; }

        //[Description("预约时间")]
        public DateTime? SubscribeTime { get; set; }

        [Description("姓名")]
        public string UserRealName { get; set; }

        [Description("身份证号")]
        public string IdCard { get; set; }

        [Description("经度")]
        public string Lng { get; set; }

        [Description("纬度")]
        public string Lat { get; set; }
    }
    public enum Schedule
    {
        申请试练 = 0,
        已试炼 = 1,
        已报名 = 2,
        科目一 = 3,
        科目二 = 4,
        科目三 = 5,
        科目四 = 6,
        已拿证 = 7,
    }
}
