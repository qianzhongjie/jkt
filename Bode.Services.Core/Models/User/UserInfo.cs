﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Bode.Services.Core.Models.Identity;
using OSharp.Core.Data;
using OSharp.Utility.Develop.T4;
using System.Collections;
using System.Collections.Generic;

namespace Bode.Services.Core.Models.User
{
    [Generate]
    [Description("用户-用户信息")]
    public class UserInfo : EntityBase<int>
    {

        [Description("头像地址")]
        public string HeadPic { get; set; }

        [Description("真实姓名")]
        public string RealName { get; set; }

        [Description("设备唯一标识号")]
        public string RegistKey { get; set; }

        [Description("性别")]
        public Sex Sex { get; set; }

        [Description("省")]
        public string Province { get; set; }

        [Description("城市")]
        public string City { get; set; }

        [Description("国家")]
        public string Country { get; set; }

        [Description("注册渠道码")]
        public string ChannelCode { get; set; }

        [Description("系统用户")]
        public virtual SysUser SysUser { get; set; }

        // [Description("生日")]
        public DateTime? Birthday { get; set; }

        [Description("qq")]
        public string Qq { get; set; }

        // [Description("上任时间")]
        public DateTime? UpTime { get; set; }

        //[Description("预约试练时间")]
        public DateTime? PracticeTime { get; set; }

        /// <summary>
        /// 登录凭据
        /// </summary>
        [Description("登录凭据")]
        public string Token { get; set; }
    }

    public enum Sex
    {
        不限 = 0,
        男 = 1,
        女 = 2
    }
}
