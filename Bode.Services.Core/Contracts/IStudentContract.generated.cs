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
using Bode.Services.Core.Dtos.Student;
using Bode.Services.Core.Models.Student;

namespace Bode.Services.Core.Contracts
{
	public partial interface IStudentContract : IScopeDependency
	{
		                #region City信息业务

                IQueryable<City> Citys { get; }

                /// <summary>
                /// 保存City信息(新增/更新)
                /// </summary>
                /// <param name="updateForeignKey">更新时是否更新外键信息</param>
                /// <param name="dtos">要保存的CityDto信息</param>
                /// <returns>业务操作集合</returns>
                Task<OperationResult> SaveCitys(bool updateForeignKey=false,params CityDto[] dtos);

                /// <summary>
                /// 删除City信息
                /// </summary>
                /// <param name="ids">要删除的Id编号</param>
                /// <returns>业务操作结果</returns>
                Task<OperationResult> DeleteCitys(params int[] ids);

                #endregion

                                #region JCU信息业务

                IQueryable<JCU> JCUs { get; }

                /// <summary>
                /// 保存JCU信息(新增/更新)
                /// </summary>
                /// <param name="updateForeignKey">更新时是否更新外键信息</param>
                /// <param name="dtos">要保存的JCUDto信息</param>
                /// <returns>业务操作集合</returns>
                Task<OperationResult> SaveJCUs(bool updateForeignKey=false,params JCUDto[] dtos);

                /// <summary>
                /// 删除JCU信息
                /// </summary>
                /// <param name="ids">要删除的Id编号</param>
                /// <returns>业务操作结果</returns>
                Task<OperationResult> DeleteJCUs(params int[] ids);

                #endregion

                                #region JcuSystem信息业务

                IQueryable<JcuSystem> JcuSystems { get; }

                /// <summary>
                /// 保存JcuSystem信息(新增/更新)
                /// </summary>
                /// <param name="updateForeignKey">更新时是否更新外键信息</param>
                /// <param name="dtos">要保存的JcuSystemDto信息</param>
                /// <returns>业务操作集合</returns>
                Task<OperationResult> SaveJcuSystems(bool updateForeignKey=false,params JcuSystemDto[] dtos);

                /// <summary>
                /// 删除JcuSystem信息
                /// </summary>
                /// <param name="ids">要删除的Id编号</param>
                /// <returns>业务操作结果</returns>
                Task<OperationResult> DeleteJcuSystems(params int[] ids);

                #endregion

                                #region PracticeEntry信息业务

                IQueryable<PracticeEntry> PracticeEntrys { get; }

                /// <summary>
                /// 保存PracticeEntry信息(新增/更新)
                /// </summary>
                /// <param name="updateForeignKey">更新时是否更新外键信息</param>
                /// <param name="dtos">要保存的PracticeEntryDto信息</param>
                /// <returns>业务操作集合</returns>
                Task<OperationResult> SavePracticeEntrys(bool updateForeignKey=false,params PracticeEntryDto[] dtos);

                /// <summary>
                /// 删除PracticeEntry信息
                /// </summary>
                /// <param name="ids">要删除的Id编号</param>
                /// <returns>业务操作结果</returns>
                Task<OperationResult> DeletePracticeEntrys(params int[] ids);

                #endregion

                                #region SiteFactory信息业务

                IQueryable<SiteFactory> SiteFactorys { get; }

                /// <summary>
                /// 保存SiteFactory信息(新增/更新)
                /// </summary>
                /// <param name="updateForeignKey">更新时是否更新外键信息</param>
                /// <param name="dtos">要保存的SiteFactoryDto信息</param>
                /// <returns>业务操作集合</returns>
                Task<OperationResult> SaveSiteFactorys(bool updateForeignKey=false,params SiteFactoryDto[] dtos);

                /// <summary>
                /// 删除SiteFactory信息
                /// </summary>
                /// <param name="ids">要删除的Id编号</param>
                /// <returns>业务操作结果</returns>
                Task<OperationResult> DeleteSiteFactorys(params int[] ids);

                #endregion

                                #region StudentInfo信息业务

                IQueryable<StudentInfo> StudentInfos { get; }

                /// <summary>
                /// 保存StudentInfo信息(新增/更新)
                /// </summary>
                /// <param name="updateForeignKey">更新时是否更新外键信息</param>
                /// <param name="dtos">要保存的StudentInfoDto信息</param>
                /// <returns>业务操作集合</returns>
                Task<OperationResult> SaveStudentInfos(bool updateForeignKey=false,params StudentInfoDto[] dtos);

                /// <summary>
                /// 删除StudentInfo信息
                /// </summary>
                /// <param name="ids">要删除的Id编号</param>
                /// <returns>业务操作结果</returns>
                Task<OperationResult> DeleteStudentInfos(params int[] ids);

                #endregion

                	}
}
