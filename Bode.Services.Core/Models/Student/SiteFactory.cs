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
    [Description("场地")]
    public class SiteFactory : EntityBase<int>
    {
        [Description("所属校区")]
        public virtual JCU JCU { get; set; }

        [Description("名称")]
        public string Name { get; set; }

        [Description("封面")]
        public string Cover { get; set; }

        [Description("地址")]
        public string Address { get; set; }

        [Description("经度")]
        public string Lng { get; set; }

        [Description("纬度")]
        public string Lat { get; set; }

        [Description("详细介绍")]
        public string Detail { get; set; }

        //public virtual ICollection<SitePic> Pics { get; set; }
    }
}
