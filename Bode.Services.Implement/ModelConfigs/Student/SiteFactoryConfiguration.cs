using Bode.Services.Core.Models.Student;
using OSharp.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bode.Services.Implement.ModelConfigs.Student
{
    public class SiteFactoryConfiguration : EntityConfigurationBase<SiteFactory, int>
    {
        public SiteFactoryConfiguration()
        {
            HasRequired(x => x.JCU).WithMany(x => x.SiteFactory);
        }
    }
}
