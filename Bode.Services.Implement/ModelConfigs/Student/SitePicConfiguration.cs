using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bode.Services.Core.Models.Student;
using OSharp.Data.Entity;

namespace Bode.Services.Implement.ModelConfigs.Student
{
    public class SitePicConfiguration : EntityConfigurationBase<SitePic, int>
    {
        public SitePicConfiguration()
        {
            HasRequired(x => x.SiteFactory).WithMany(x => x.Pics);
        }
    }
}
