﻿// <autogenerated>
//   This file was generated by T4 code generator ContractsCodeScript.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>


using System;
using System.Linq;
using OSharp.Core;
using OSharp.Utility.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using OSharp.Core.Dependency;
using Bode.Services.Core.Dtos.Wx;
using Bode.Services.Core.Models.Wx;

namespace Bode.Services.Core.Contracts
{
	public partial interface IWxContract : IScopeDependency
	{
		                #region TokenCode信息业务

                IQueryable<TokenCode> TokenCodes { get; }

                /// <summary>
                /// 保存TokenCode信息(新增/更新)
                /// </summary>
                /// <param name="updateForeignKey">更新时是否更新外键信息</param>
                /// <param name="dtos">要保存的TokenCodeDto信息</param>
                /// <returns>业务操作集合</returns>
                Task<OperationResult> SaveTokenCodes(bool updateForeignKey=false,params TokenCodeDto[] dtos);

                /// <summary>
                /// 删除TokenCode信息
                /// </summary>
                /// <param name="ids">要删除的Id编号</param>
                /// <returns>业务操作结果</returns>
                Task<OperationResult> DeleteTokenCodes(params int[] ids);

                #endregion

                	}
}
