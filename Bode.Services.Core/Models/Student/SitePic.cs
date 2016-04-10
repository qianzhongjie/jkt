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
    public class SitePic : EntityBase<int>
    {
        [Description("地址")]
        public string Path { get; set; }

    }
}
