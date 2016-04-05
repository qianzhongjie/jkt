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
    [Description("城市")]
    public class City : EntityBase<int>
    {
        public City()
        {
            Jcu = new List<JCU>();
        }
        [Description("名称")]
        public string Name { get; set; }

        public ICollection<JCU> Jcu { get; set; }

    }
}
