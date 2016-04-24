using Bode.Services.Core.Models.User;
using OSharp.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bode.Services.Core.Dtos.Student;
using Bode.Services.Core.Models.Student;
using OSharp.Utility.Data;

namespace Bode.Services.Implement.Services
{
    public partial class StudentService
    {
        public IRepository<UserInfo, int> UserInfoRepo { protected get; set; }

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
    }
}
