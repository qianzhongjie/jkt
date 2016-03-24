﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bode.Services.Core.Contracts;
using Bode.Services.Core.Dtos.Identity;
using Bode.Services.Core.Dtos.Security;
using Bode.Services.Core.Models.Identity;
using OSharp.Core.Security;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Extensions;
using OSharp.Utility.Filter;
using OSharp.Web.Mvc.Security;
using OSharp.Web.Mvc.UI;
using OSharp.Web.Mvc;
using OSharp.Core.Logging;

namespace Bode.Web.Areas.Admin.Controllers
{
    //[Authorize]
    [Description("系统管理")]
    public class IdentityController : AdminBaseController
    {
        public IIdentityContract IdentityContract { get; set; }
        public IUserContract UserContract { get; set; }
        public ISecurityContract SecurityContract { get; set; }
        /// <summary>
        /// 获取或设置 日志业务对象
        /// </summary>
        public ILoggingContract LoggingContract { get; set; }
        #region Ajax功能

        #region 组织机构

        [AjaxOnly]
        [Description("获取组织机构数据")]
        public ActionResult GetOrganizationData(int? parentId)
        {
            int total;
            GridRequest request = new GridRequest(Request);
            if (parentId.HasValue && parentId.Value > 0)
            {
                request.FilterGroup.Rules.Add(new FilterRule("Parent.Id", parentId.Value));
            }

            var datas =
                GetQueryData<SysOrganization, int>(IdentityContract.Organizations.Include(p => p.Parent), out total,
                    request).Select(m => new
                    {
                        m.Id,
                        m.Name,
                        m.Remark,
                        m.SortCode,
                        ParentId = m.Parent == null ? 0 : m.Parent.Id,
                        m.CreatedTime
                    });
            return Json(new GridData<object>(datas, total), JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpPost]
        [Description("保存组织机构数据")]
        public async Task<ActionResult> SaveOrganizationData(SysOrganizationDto[] dtos)
        {
            dtos.CheckNotNull("dtos");
            OperationResult result = await IdentityContract.SaveOrganizations(dtos);
            return Json(result.ToAjaxResult());
        }

        [AjaxOnly]
        [HttpPost]
        [Description("删除组织机构数据")]
        public async Task<ActionResult> DeleteOrganizations(int[] ids)
        {
            ids.CheckNotNull("ids");
            OperationResult result = await Task.Run(() => IdentityContract.DeleteOrganizations(ids));
            return Json(result.ToAjaxResult());
        }

        #endregion

        #region 角色

        [AjaxOnly]
        [Description("获取组织机构树数据")]
        public ActionResult GetOrganizationTree()
        {
            var data = IdentityContract.Organizations.Include(p => p.Parent).Select(p => new
            {
                value = p.Id,
                text = p.Name,
                parentId = p.Parent == null ? 0 : p.Parent.Id
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [AjaxOnly]
        [Description("获取角色数据")]
        public ActionResult GetRoleData(int? organizationId)
        {
            int total;
            GridRequest request = new GridRequest(Request);
            if (organizationId.HasValue && organizationId.Value > 0)
            {
                request.FilterGroup.Rules.Add(new FilterRule("Organization.Id", organizationId.Value));
            }

            var datas = GetQueryData<SysRole, int>(IdentityContract.Roles.Include(p => p.Organization), out total, request).Select(m => new
            {
                m.Id,
                m.Name,
                m.Remark,
                m.IsAdmin,
                m.IsLocked,
                m.CreatedTime,
                OrganizationId = m.Organization == null ? 0 : m.Organization.Id,
                OrganizationName = m.Organization.Name
            });
            return Json(new GridData<object>(datas, total), JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpPost]
        [Description("保存角色数据")]
        public async Task<ActionResult> SaveRoleData(SysRoleDto[] dtos)
        {
            dtos.CheckNotNull("dtos");
            OperationResult result = await IdentityContract.SaveRoles(dtos);
            return Json(result.ToAjaxResult());
        }

        [AjaxOnly]
        [HttpPost]
        [Description("删除角色数据")]
        public async Task<ActionResult> DeleteRoles(int[] ids)
        {
            ids.CheckNotNull("ids");
            OperationResult result = await IdentityContract.DeleteRoles(ids);
            return Json(result.ToAjaxResult());
        }

        [AjaxOnly]
        [Description("获取所有角色简要信息")]
        public ActionResult GetAllRoleBriefs(int? organizationId)
        {
            var data = IdentityContract.Roles.Select(p => new
            {
                p.Id,
                p.Name,
                OrganizationId = p.Organization == null ? 0 : p.Organization.Id
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [Description("获取角色功能Id")]
        public ActionResult GetRoleFuncIds(int roleId)
        {
            var funcIds = SecurityContract
                .FunctionRoleMaps.Where(p => p.Role.Id == roleId)
                .Include(p => p.Function)
                .Select(p => p.Function.Id)
                .ToList();
            return Json(funcIds, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [Description("设置角色功能")]
        public async Task<ActionResult> SetRoleFunctions(int roleId, string strFuncIds)
        {
            var funcIds = JsonToEntity<Guid[]>(strFuncIds);
            var result = await SecurityContract.SetRoleFunctions(roleId, funcIds);
            return Json(result.ToAjaxResult());
        }

        #endregion

        #region 用户

        [AjaxOnly]
        [Description("获取用户数据")]
        public ActionResult GetUserData()
        {
            int total;
            GridRequest request = new GridRequest(Request);
            var datas = GetQueryData<SysUser, int>(IdentityContract.Users.Where(p => p.UserType == UserType.系统用户), out total, request).Select(m => new
            {
                m.Id,
                m.UserName,
                m.NickName,
                Password = m.PasswordHash,
                m.Email,
                m.IsLocked,
                m.CreatedTime,
            });
            return Json(new GridData<object>(datas, total), JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpPost]
        [Description("保存用户数据")]
        public async Task<ActionResult> SaveUserData(SysUserDto[] dtos)
        {
            dtos.CheckNotNull("dtos");
            OperationResult result = await IdentityContract.SaveUsers(dtos);
            return Json(result.ToAjaxResult());
        }

        [AjaxOnly]
        [HttpPost]
        [Description("删除用户数据")]
        public async Task<ActionResult> DeleteUsers(int[] ids)
        {
            ids.CheckNotNull("ids");
            OperationResult result = await IdentityContract.DeleteUsers(ids);
            return Json(result.ToAjaxResult());
        }

        [AjaxOnly]
        [Description("获取用户角色Id")]
        public ActionResult GetUserRoleIds(int userId)
        {
            var roleIds = IdentityContract.UserRoleMaps
                .Where(p => p.User.Id == userId)
                .Include(p => p.Role)
                .Select(p => p.Role.Id)
                .ToList();
            return Json(roleIds, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpPost]
        [Description("设置用户角色")]
        public async Task<ActionResult> SetUserRoles(int userId, string strRoleIds)
        {
            var roleIds = JsonToEntity<int[]>(strRoleIds);
            var result = await IdentityContract.SetUserRoles(userId, roleIds);
            return Json(result.ToAjaxResult());
        }

        [HttpPost]
        [Description("添加管理员")]
        public async Task<ActionResult> AddSysUser(string account, string pwd)
        {
            var count = IdentityContract.Users.Count(x => x.UserName == account);
            if (count > 0) return Json(new OperationResult(OperationResultType.QueryNull, "此账号已存在").ToAjaxResult());
            SysUserDto dto = new SysUserDto
            {
                UserName = account,
                Password = pwd
            };
            OperationResult result = await IdentityContract.SaveUsers(dto);
            return Json(result.ToAjaxResult());
        }

        [HttpPost]
        [Description("保存修改密码数据")]
        public async Task<ActionResult> EidtPassword(string oldPwd, string newPwd)
        {
            if (string.IsNullOrWhiteSpace(User.Identity.Name))
                return Json(new OperationResult(OperationResultType.QueryNull, "登录信息丢失，请重新登录").ToAjaxResult());
            var result = await IdentityContract.ResetPassword(User.Identity.Name, oldPwd, newPwd);
            return Json(result.ToAjaxResult());
        }
        #endregion

        #region 功能

        [AjaxOnly]
        [Description("获取控制器数据")]
        public ActionResult GetControllers()
        {
            var areas = SecurityContract.Functions.Select(p => new
            {
                value = p.Area == null || p.Area == "" ? "Home" : p.Area,
                text = p.Area == null || p.Area == "" ? "Home" : p.Area,
                parentId = "0"
            }).Distinct().ToList();

            var data = SecurityContract.Functions.Where(p => p.IsController && (p.PlatformToken == PlatformToken.Mvc || p.PlatformToken == PlatformToken.WebApi)).Select(p => new
            {
                value = p.Id.ToString(),
                text = p.Name,
                parentId = p.Area == null || p.Area == "" ? "Home" : p.Area,
            }).ToList();
            data.AddRange(areas);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [Description("获取所有功能简要")]
        public ActionResult GetAllActionBriefs()
        {
            var controllers = SecurityContract.Functions.Where(p => p.IsController).Select(p => new
            {
                p.Id,
                p.Area,
                p.Controller,
                p.PlatformToken
            }).ToList();

            var actions = SecurityContract.Functions.ToList().Select(p => new
            {
                p.Id,
                p.Name,
                ControllerId = controllers.Single(m => m.Area == p.Area && m.Controller == p.Controller && m.PlatformToken == p.PlatformToken).Id,
                IsMenu = !p.IsAjax && p.IsMenu,
                p.IsController
            }).OrderByDescending(p => p.IsController).ThenByDescending(p => p.IsMenu);

            return Json(actions, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [Description("获取功能数据")]
        public async Task<ActionResult> GetFunctionData(Guid controllerId)
        {
            var controller = await SecurityContract.Functions.SingleOrDefaultAsync(p => p.Id == controllerId) ?? new Function();

            int total;
            GridRequest request = new GridRequest(Request);
            var datas =
                GetQueryData<Function, Guid>(
                    SecurityContract.Functions.Where(
                        p =>
                            p.Area == controller.Area && p.Controller == controller.Controller &&
                            p.PlatformToken == controller.PlatformToken),
                    out total, request).Select(m => new
                    {
                        m.Id,
                        m.Name,
                        m.OrderNo,
                        m.FunctionType,
                        m.OperateLogEnabled,
                        m.DataLogEnabled,
                        m.Area,
                        m.Controller,
                        m.Action,
                        m.IsController,
                        ControllerId = controllerId,
                        m.IsAjax,
                        m.IsLocked,
                        m.IsMenu
                    });
            return Json(new GridData<object>(datas, total), JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpPost]
        [Description("保存功能数据")]
        public async Task<ActionResult> EditFunctionData(FunctionDto[] dtos)
        {
            dtos.CheckNotNull("dtos");
            OperationResult result = await SecurityContract.EditFunctions(dtos);
            return Json(result.ToAjaxResult());
        }

        [AjaxOnly]
        [HttpPost]
        [Description("删除功能数据")]
        public async Task<ActionResult> DeleteFunctions(Guid[] ids)
        {
            ids.CheckNotNull("ids");
            OperationResult result = await SecurityContract.DeleteFunctions(ids);
            return Json(result.ToAjaxResult());
        }

        #endregion

        #region 实体

        [AjaxOnly]
        [Description("获取实体数据")]
        public ActionResult GetEntityInfoData()
        {
            int total;
            GridRequest request = new GridRequest(Request);
            if (request.PageCondition.SortConditions.Length == 0)
            {
                request.PageCondition.SortConditions = new[]
                {
                    new SortCondition("ClassName")
                };
            }
            var datas = GetQueryData<EntityInfo, Guid>(SecurityContract.EntityInfos,
                    out total, request).Select(m => new
                    {
                        m.Id,
                        m.Name,
                        m.ClassName,
                        m.DataLogEnabled
                    });
            return Json(new GridData<object>(datas, total), JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpPost]
        [Description("编辑实体数据")]
        public ActionResult EditEntityInfoData(EntityInfoDto[] dtos)
        {
            dtos.CheckNotNull("dtos");
            OperationResult result = SecurityContract.EditEntityInfos(dtos);
            return Json(result.ToAjaxResult());
        }

        #endregion

        #region 操作日志

        [AjaxOnly]
        [Description("获取操作日志数据")]
        public ActionResult GetOperateLogData()
        {
            int total;
            GridRequest request = new GridRequest(Request);
            if (request.PageCondition.SortConditions.Length == 0)
            {
                request.PageCondition.SortConditions = new[] { new SortCondition("CreatedTime", ListSortDirection.Descending) };
            }
            var datas = GetQueryData<OperateLog, int>(LoggingContract.OperateLogs.Include(p => p.DataLogs), out total, request).Select(m => new
            {
                m.Id,
                m.FunctionName,
                m.Operator.UserId,
                m.Operator.UserName,
                m.Operator.Ip,
                m.CreatedTime
            }).ToList();
            return Json(new GridData<object>(datas, total), JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpPost]
        [Description("删除操作日志数据")]
        public ActionResult DeleteOperateLogs(int[] ids)
        {
            ids.CheckNotNull("ids");
            OperationResult result = LoggingContract.DeleteOperateLogs(ids);
            return Json(result.ToAjaxResult());
        }

        [AjaxOnly]
        [Description("获取数据日志数据")]
        public ActionResult GetDataLogData(int operateLogId)
        {
            var data = LoggingContract.DataLogs.Where(p => p.OperateLog != null && p.OperateLog.Id == operateLogId).Select(m => new
            {
                m.Id,
                m.Name,
                m.EntityName,
                m.OperateType,
                LogItems = m.LogItems.Select(n => new
                {
                    n.Field,
                    n.FieldName,
                    n.OriginalValue,
                    n.NewValue,
                    n.DataType
                })
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #endregion

        #region 视图页面

        [Description("组织结构列表")]
        public ActionResult OrganizationList()
        {
            return View();
        }

        [Description("角色列表")]
        public ActionResult RoleList()
        {
            return View();
        }

        [Description("用户列表")]
        public ActionResult UserList()
        {
            return View();
        }

        [Description("功能列表")]
        public ActionResult FunctionList()
        {
            ViewBag.FunctionTypes = typeof(FunctionType).ToDictionary().Select(p => new
            {
                val = p.Key,
                text = p.Value
            }).ToList();

            return View();
        }

        [Description("实体列表")]
        public ActionResult EntityInfoList()
        {
            return View();
        }

        [Description("操作日志列表")]
        public ActionResult OperateLogList()
        {
            ViewBag.OperatingTypes = typeof(OperatingType).ToDictionary().Select(p => new
            {
                val = p.Key,
                text = p.Value
            }).ToList();
            return View();
        }

        [Description("修改密码")]
        public ActionResult Epassword()
        {
            return View();
        }

        #endregion
    }
}