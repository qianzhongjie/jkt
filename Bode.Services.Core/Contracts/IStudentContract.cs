using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bode.Services.Core.Dtos.Student;
using Bode.Services.Core.Models.Student;
using OSharp.Utility.Data;

namespace Bode.Services.Core.Contracts
{
    public partial interface IStudentContract
    {
        /// <summary>
        /// 添加场地
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="pic"></param>
        /// <returns></returns>
        Task<OperationResult> AddFactory(SiteFactoryDto dto, SitePic[] pic);
    }
}
