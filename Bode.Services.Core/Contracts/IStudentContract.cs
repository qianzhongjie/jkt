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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        Task<OperationResult> AddSutdent(StudentInfoDto dto, DateTime time);

        /// <summary>
        /// 保存StudentInfo信息(新增/更新)
        /// </summary>
        /// <param name="updateForeignKey">更新时是否更新外键信息</param>
        /// <param name="dtos">要保存的StudentInfoDto信息</param>
        /// <returns>业务操作集合</returns>
        Task<OperationResult> AddEditStudentInfos(bool updateForeignKey = false,
           params StudentInfoDto[] dtos);

        /// <summary>
        /// 更改学yuan校区
        /// </summary>
        /// <param name="jcuId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        OperationResult ChangeJcu(int jcuId, string openId);
    }
}
