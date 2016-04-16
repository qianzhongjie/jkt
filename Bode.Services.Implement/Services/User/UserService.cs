using System.Threading.Tasks;
using Bode.Services.Core.Contracts;
using Bode.Services.Core.Dtos.User;
using Bode.Services.Core.Models.Identity;
using System.Linq;
using Bode.Plugin.Core.SMS;
using Bode.Services.Implement.Permissions.Identity;
using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;

namespace Bode.Services.Implement.Services
{
    public partial class UserService
    {
        public IRepository<SysUser, int> SysUserRepo { protected get; set; }

        public IIdentityContract IdentityContract { protected get; set; }

        /// <summary>
        /// 获取或设置 用户管理器
        /// </summary>
        public UserManager UserManager { get; set; }

        /// <summary>
        /// 获取或设置 用户存储器
        /// </summary>
        //public UserStore UserStore { get; set; }

        public ISms Sms { get; set; }


        /// <summary>
        /// 编辑UserInfo信息
        /// </summary>
        /// <param name="dtos">要更新的UserInfoEditDto信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> EditUserInfos(params UserInfoEditDto[] dtos)
        {
            dtos.CheckNotNull("dtos");

            var result = UserInfoRepo.Update(dtos, updateFunc: (dto, userInfo) =>
            {
                var sysUser = userInfo.SysUser;
                userInfo.Id = dto.UserInfoId;
                sysUser.NickName = dto.NickName;
                sysUser.PhoneNumber = dto.PhoneNumber;
                return userInfo;
            });
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 修改个人资料
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public OperationResult SaveUserDetail(UserInfoEditDto dto)
        {
            UserInfoRepo.UnitOfWork.TransactionEnabled = true;
            var userinfo = UserInfoRepo.GetByKey(dto.UserInfoId);
            userinfo.SysUser.NickName = dto.NickName;
            userinfo.Qq = dto.Qq;
            userinfo.RealName = dto.RealName;
            userinfo.HeadPic = dto.HeadPic;
            userinfo.Sex = dto.Sex;
            userinfo.SysUser.PhoneNumber = dto.PhoneNumber;
            UserInfoRepo.Update(userinfo);
            UserInfoRepo.UnitOfWork.SaveChanges();
            return new OperationResult(OperationResultType.Success, "修改成功");
        }
    }
}
