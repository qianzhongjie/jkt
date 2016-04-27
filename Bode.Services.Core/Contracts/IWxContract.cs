using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bode.Services.Core.Dtos.Wx;
using OSharp.Utility.Data;

namespace Bode.Services.Core.Contracts
{
    public partial interface IWxContract
    {
        OperationResult AddTokenCode(TokenCodeDto dto);
    }
}
