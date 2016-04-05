using Bode.Services.Core.Models.Student;
using OSharp.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bode.Services.Implement.ModelConfigs.Student
{
    public class StudentInfoConfiguration : EntityConfigurationBase<StudentInfo, int>
    {
        public StudentInfoConfiguration()
        {
            HasRequired(x => x.UserInfo);
            HasRequired(x => x.Jcu).WithMany(x => x.StudenInfos);
        }
    }
}
