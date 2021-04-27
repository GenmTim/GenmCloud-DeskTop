using GenmCloud.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenmCloud.Shared.HttpContact.Request
{
    public class UserLoginRequest : BaseRequest
    {
        public override string route { get => "api/User/Login"; }

        public LoginDto Parameter { get; set; } 
    }
}
