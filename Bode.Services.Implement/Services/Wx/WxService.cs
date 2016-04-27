using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bode.Services.Core.Dtos.Wx;
using Bode.Services.Core.Models.Wx;
using OSharp.Utility.Data;

namespace Bode.Services.Implement.Services
{
    public partial class WxService
    {
        public OperationResult AddTokenCode(TokenCodeDto dto)

        {
            TokenCodeRepo.UnitOfWork.TransactionEnabled = true;
            var m = new TokenCode()
            {
                Value = dto.Value,
                Type = dto.Type
            };
            TokenCodeRepo.Insert(m);
            TokenCodeRepo.UnitOfWork.SaveChanges();
            return new OperationResult(OperationResultType.Success, "添加成功", m.Id);
        }
    }
}
