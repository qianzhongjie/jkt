﻿// <autogenerated>
//   This file was generated by T4 code generator DtosCodeScript.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>


using OSharp.Core.Data;
using System.ComponentModel;
using Bode.Services.Core.Models.Bnaners;

namespace Bode.Services.Core.Dtos.Bnaners
{
	public partial class BnanerDto: IAddDto, IEditDto<int>
	{
        public System.Int32 Id { get; set; }
                            public System.String Path { get; set; }
                                        public System.String Url { get; set; }
                                        public System.Boolean IsDisPlay { get; set; }
                    	}
}