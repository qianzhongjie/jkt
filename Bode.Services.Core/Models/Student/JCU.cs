using Bode.Services.Core.Models.Identity;
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
    [Description("校区")]
    public class JCU : EntityBase<int>
    {
        public JCU()
        {
            SysUser = new List<JcuSystem>();
            StudenInfos = new List<StudentInfo>();
        }

        [Description("名称")]
        public string Name { get; set; }

        [Description("经度")]
        public string Log { get; set; }

        [Description("纬度")]
        public string Lat { get; set; }

        [Description("地址")]
        public string Address { get; set; }

        [Description("所属城市")]
        public virtual City City { get; set; }

        //校区管理员
        public virtual ICollection<JcuSystem> SysUser { get; set; }
        //校区的学员
        public virtual ICollection<StudentInfo> StudenInfos { get; set; }
    }
}
