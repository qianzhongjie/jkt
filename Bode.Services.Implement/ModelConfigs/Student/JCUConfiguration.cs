using Bode.Services.Core.Models.Student;
using OSharp.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bode.Services.Implement.ModelConfigs.Student
{
    public class JCUConfiguration : EntityConfigurationBase<JCU, int>
    {
        public JCUConfiguration()
        {
            HasRequired(x => x.City).WithMany(x => x.Jcu);
        }
    }
}
