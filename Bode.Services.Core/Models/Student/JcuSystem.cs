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
    [Description("校区经理")]
    public class JcuSystem : EntityBase<int>
    {
        //public JcuSystem()
        //{
        //    Jcus = new List<JCU>();
        //}
        [Description("关联用户")]
        public virtual UserInfo SystemInfo { get; set; }

        [Description("校区")]
        public virtual JCU Jcus { get; set; }
    }
}
