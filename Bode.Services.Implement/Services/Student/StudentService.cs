using Bode.Services.Core.Models.User;
using OSharp.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bode.Services.Implement.Services
{
    public partial class StudentService
    {
        public IRepository<UserInfo, int> UserInfoRepo { protected get; set; }
    }
}
