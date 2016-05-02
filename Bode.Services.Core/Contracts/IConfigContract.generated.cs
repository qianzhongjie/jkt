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
using Bode.Services.Core.Dtos.Config;
using Bode.Services.Core.Models.Config;

namespace Bode.Services.Core.Contracts
{
	public partial interface IConfigContract : IScopeDependency
	{
		                #region ContactConfig信息业务

                IQueryable<ContactConfig> ContactConfigs { get; }

                /// <summary>
                /// 保存ContactConfig信息(新增/更新)
                /// </summary>
                /// <param name="updateForeignKey">更新时是否更新外键信息</param>
                /// <param name="dtos">要保存的ContactConfigDto信息</param>
                /// <returns>业务操作集合</returns>
                Task<OperationResult> SaveContactConfigs(bool updateForeignKey=false,params ContactConfigDto[] dtos);

                /// <summary>
                /// 删除ContactConfig信息
                /// </summary>
                /// <param name="ids">要删除的Id编号</param>
                /// <returns>业务操作结果</returns>
                Task<OperationResult> DeleteContactConfigs(params int[] ids);

                #endregion

                	}
}
