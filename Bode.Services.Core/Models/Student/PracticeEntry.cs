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
    [Description("申请试练实体")]
    public class PracticeEntry : EntityBase<int>
    {


        [Description("关联用户")]
        public virtual UserInfo UserInfo { get; set; }

        [Description("预约时间")]
        public DateTime SubscribeTime { get; set; }

        [Description("联系方式")]
        public string PhoneNo { get; set; }

        [Description("姓名")]
        public string Name { get; set; }
    }
}
