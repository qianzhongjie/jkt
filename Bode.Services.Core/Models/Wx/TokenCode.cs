using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSharp.Core.Data;
using OSharp.Utility.Develop.T4;

namespace Bode.Services.Core.Models.Wx
{
    [Generate]
    [Description("城市")]
    public class TokenCode : EntityBase<int>
    {
        [Description("值")]
        public string Value { get; set; }

        [Description("类型")]
        public  TokenType Type { get; set; }
    }

    public enum TokenType
    {
        Code = 0,
        Token = 1,
        Other = 2
    }
}
