﻿// <autogenerated>
//   This file was generated by T4 code generator DtosCodeScript.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>


using OSharp.Core.Data;
using System.ComponentModel;
using Bode.Services.Core.Models.User;

namespace Bode.Services.Core.Dtos.User
{
	public partial class ValidateCodeDto: IAddDto, IEditDto<int>
	{
        public System.Int32 Id { get; set; }
                            public System.String CodeKey { get; set; }
                                        public Bode.Services.Core.Models.User.CodeType CodeType { get; set; }
                                        public Bode.Services.Core.Models.User.ValidateType ValidateType { get; set; }
                                        public System.String Code { get; set; }
                    	}
}
