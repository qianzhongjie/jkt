using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSharp.Core.Data;
using OSharp.Utility.Develop.T4;

namespace Bode.Services.Core.Models.Bnaners
{
    [Generate]
    [Description("广告图")]
    public class Bnaner : EntityBase<int>
    {
        [Description("地址")]
        public string Path { get; set; }

        [Description("url")]
        public string Url { get; set; }

        [Description("是否显示")]
        public bool IsDisPlay { get; set; }
    }

}
