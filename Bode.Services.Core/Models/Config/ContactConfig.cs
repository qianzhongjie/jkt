using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSharp.Core.Data;
using OSharp.Utility.Develop.T4;

namespace Bode.Services.Core.Models.Config
{
    [Generate]
    [Description("联系方式配置表")]
    public class ContactConfig : EntityBase<int>
    {
        [Description("值")]
        public string Value { get; set; }

        [Description("类型")]
        public ContactType Type { get; set; }
    }

    public enum ContactType
    {
        手机 = 0,
        QQ = 1,
        微信 = 2
    }
}
