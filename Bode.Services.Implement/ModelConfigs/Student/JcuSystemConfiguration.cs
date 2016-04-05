using Bode.Services.Core.Models.Student;
using OSharp.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bode.Services.Implement.ModelConfigs.Student
{
    public class JcuSystemConfiguration : EntityConfigurationBase<JcuSystem, int>
    {
        public JcuSystemConfiguration()
        {
            HasRequired(x => x.SystemInfo);
            HasRequired(x => x.Jcus).WithMany(x => x.SysUser);
        }
    }
}
