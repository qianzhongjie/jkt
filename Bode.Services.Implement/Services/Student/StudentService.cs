using Bode.Services.Core.Models.User;
using OSharp.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bode.Services.Core.Dtos.Student;
using Bode.Services.Core.Models.Identity;
using Bode.Services.Core.Models.Student;
using OSharp.Utility;
using OSharp.Utility.Data;

namespace Bode.Services.Implement.Services
{
    public partial class StudentService
    {
        public IRepository<UserInfo, int> UserInfoRepo { protected get; set; }
        public IRepository<SysUser, int> SysUserRepo { protected get; set; }
        /// <summary>
        /// 添加场地
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="pic"></param>
        /// <returns></returns>
        public async Task<OperationResult> AddFactory(SiteFactoryDto dto, SitePic[] pic)
        {
            SiteFactoryRepo.UnitOfWork.TransactionEnabled = true;
            var siteFactory = new SiteFactory
            {
                JCU = JCURepo.GetByKey(dto.JCUId),
                Name = dto.Name,
                Cover = dto.Cover,
                Address = dto.Address,
                Lng = dto.Lng,
                Lat = dto.Lat,
                Detail = dto.Detail,
                // Pics = pic
            };
            var result = 0;
            if (dto.Id == 0)
            {
                result = SiteFactoryRepo.Insert(siteFactory);
            }
            else
            {
                siteFactory = SiteFactoryRepo.GetByKey(dto.Id);
                siteFactory.JCU = JCURepo.GetByKey(dto.JCUId);
                siteFactory.Name = dto.Name;
                siteFactory.Cover = dto.Cover;
                siteFactory.Address = dto.Address;
                siteFactory.Lng = dto.Lng;
                siteFactory.Lat = dto.Lat;
                siteFactory.Detail = dto.Detail;
                // SitePicRepo.Recycle(siteFactory.Pics);
                //siteFactory.Pics = pic;
                result = SiteFactoryRepo.Update(siteFactory);
            }
            await SiteFactoryRepo.UnitOfWork.SaveChangesAsync();
            return new OperationResult(OperationResultType.Success, "操作成功");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public async Task<OperationResult> AddSutdent(StudentInfoDto dto, DateTime time)
        {
            StudentInfoRepo.UnitOfWork.TransactionEnabled = true;
            StudentInfo student = new StudentInfo();
            if (dto.JcuSystemId > 0)
            {
                student.JcuSystem = JcuSystemRepo.GetByKey(dto.JcuSystemId);
            }
            student.Jcu = JCURepo.GetByKey(dto.JcuId);
            student.ScheduleState = dto.ScheduleState;
            student.SubscribeTime = time;
            student.UserRealName = dto.UserRealName;
            student.Lat = dto.Lat;
            student.Lng = dto.Lng;
            var user = UserInfoRepo.GetByKey(dto.UserInfoId);
            student.UserInfo = user;
            StudentInfoRepo.Insert(student);
            SysUserRepo.UnitOfWork.TransactionEnabled = true;
            var sysUser = user.SysUser;
            sysUser.PhoneNumber = student.IdCard;
            SysUserRepo.Update(sysUser);
            await SysUserRepo.UnitOfWork.SaveChangesAsync();
            await StudentInfoRepo.UnitOfWork.SaveChangesAsync();
            return new OperationResult(OperationResultType.Success, "预约成功,", student.Id);
        }

        /// <summary>
        /// 保存StudentInfo信息(新增/更新)
        /// </summary>
        /// <param name="updateForeignKey">更新时是否更新外键信息</param>
        /// <param name="dtos">要保存的StudentInfoDto信息</param>
        /// <returns>业务操作集合</returns>
        public async Task<OperationResult> AddEditStudentInfos(bool updateForeignKey = false, params StudentInfoDto[] dtos)
        {
            try
            {
                dtos.CheckNotNull("dtos");
                StudentInfoRepo.UnitOfWork.TransactionEnabled = true;

                if (dtos.Length > 0)
                {
                    foreach (var dto in dtos)
                    {
                        var model = StudentInfoRepo.GetByKey(dto.Id);
                        if (dto.JcuSystemId > 0)
                        {
                            model.JcuSystem = JcuSystemRepo.GetByKey(dto.JcuSystemId);
                        }
                        else
                        {
                            model.JcuSystem = null;
                        }
                        model.IdCard = dto.IdCard;
                        model.UserRealName = dto.UserRealName;
                        model.ScheduleState = dto.ScheduleState;
                        await StudentInfoRepo.UpdateAsync(model);
                        await StudentInfoRepo.UnitOfWork.SaveChangesAsync();
                    }
                }
                return new OperationResult(OperationResultType.Success, "保存成功");
            }
            catch (Exception e)
            {
                return new OperationResult(OperationResultType.Error, e.Message);
            }
        }

        /// <summary>
        /// 更改学yuan校区
        /// </summary>
        /// <param name="jcuId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public OperationResult ChangeJcu(int jcuId, string openId)
        {
            StudentInfoRepo.UnitOfWork.TransactionEnabled = true;
            var user = StudentInfoRepo.GetByPredicate(x => x.UserInfo.Token == openId);
            if (!user.Any()) return new OperationResult(OperationResultType.Error, "服务器繁忙,请稍候再试");
            var u = user.First();
            u.Jcu = JCURepo.GetByKey(jcuId);
            StudentInfoRepo.Update(u);
            StudentInfoRepo.UnitOfWork.SaveChanges();
            return new OperationResult(OperationResultType.Success, "修改成功");
        }
    }
}
