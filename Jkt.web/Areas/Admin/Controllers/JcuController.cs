﻿using Bode.Services.Core.Contracts;
using Bode.Services.Core.Dtos.Student;
using Bode.Services.Core.Models.Student;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Web.Mvc;
using OSharp.Web.Mvc.Security;
using OSharp.Web.Mvc.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bode.Common.Result;
using OSharp.Core.Data;

namespace Bode.Web.Areas.Admin.Controllers
{

    [Description("校区管理")]
    public class JcuController : AdminBaseController
    {
        public IStudentContract studentContract { get; set; }
        public IRepository<City, int> CityRepo { protected get; set; }
        [AjaxOnly]
        [Description("获取校区信息")]
        public ActionResult GetJcuData(int cityId)
        {
            int total;
            var student = studentContract.StudentInfos.Where(x => x.ScheduleState > Schedule.已试炼);
            GridRequest request = new GridRequest(Request);
            var datas = GetQueryData<JCU, int>(studentContract.JCUs.Where(x => x.City.Id == cityId), out total,
                    request).Select(m => new
                    {
                        m.CreatedTime,
                        m.Id,
                        m.Name,
                        m.Address,
                        m.Lat,
                        m.Log,
                        CityId = m.City.Id,
                        CityName = m.City.Name,
                        Sum = student.Count(x => x.Jcu.Id == m.Id)
                    });
            return Json(new GridData<object>(datas, total), JsonRequestBehavior.AllowGet);
        }

        // [AjaxOnly]
        [HttpPost]
        [Description("保存校区数据")]
        public async Task<ActionResult> SaveJcuData(JCUDto[] dtos)
        {
            dtos.CheckNotNull("dtos");

            //List<JCUDto> dtoList = new List<JCUDto>();
            foreach (var dto in dtos)
            {
                var model = studentContract.JCUs.Where(x => x.Name == dto.Name);
                if (model.Any())
                {
                    if (dto.Id == 0)
                    {
                        return Json(new OperationResult(OperationResultType.QueryNull, "此名称已存在,请不要重复添加").ToAjaxResult());
                    }
                    else if (model.First().Id != dto.Id)
                    {
                        return Json(new OperationResult(OperationResultType.QueryNull, "此名称已存在,请不要重复添加").ToAjaxResult());
                    }
                }
                //if (string.IsNullOrWhiteSpace(dto.Address))
                //{
                //    return Json(new OperationResult(OperationResultType.Error, "必须输入地址", "").ToAjaxResult());
                //}
                //if (string.IsNullOrWhiteSpace(dto.Name))
                //{
                //    return Json(new OperationResult(OperationResultType.Error, "必须输入学校名称", "").ToAjaxResult());
                //}
                //// var cityName = CityRepo.GetByKey(dto.CityId);
                //var reLng = BaiDuResult.GetLngLat(dto.Name).result.location;
                //dto.Lat = reLng.lat.ToString();
                //dto.Log = reLng.lng.ToString();
                //dtoList.Add(dto);
            }

            OperationResult result = await studentContract.SaveJCUs(dtos: dtos.ToArray());
            return Json(result.ToAjaxResult());
        }

        [AjaxOnly]
        [HttpPost]
        [Description("删除校区数据")]
        public async Task<ActionResult> DeleteJcu(int[] ids)
        {
            ids.CheckNotNull("ids");
            foreach (var id in ids)
            {
                var model = studentContract.StudentInfos.Where(x => x.Jcu.Id == id);
                if (model.Any())
                {
                    return Json(new OperationResult(OperationResultType.Error, "此校区下还有学员，不能删除"));
                }
            }
            OperationResult result = await Task.Run(() => studentContract.DeleteJCUs(ids));
            return Json(result.ToAjaxResult());
        }
        // GET: Admin/Puc
        [Description("校区列表")]
        public ActionResult JcuList()
        {
            return View();
        }

        [Description("添加校区视图")]
        public ActionResult EditAddJcuView(int cityId)
        {
            ViewBag.CityId = cityId;
            return View();
        }

        [Description("编辑校区试图")]
        public ActionResult EditJcuView(int id)
        {
            ViewBag.DataJcu = studentContract.JCUs.Single(x => x.Id == id);
            return View();
        }

        [AjaxOnly]
        [Description("获取城市树数据")]
        public ActionResult GetCityTree()
        {
            var data = studentContract.Citys.Select(p => new
            {
                value = p.Id,
                text = p.Name,
                parentId = 0
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}