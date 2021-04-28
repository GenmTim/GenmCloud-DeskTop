using GenmCloud.Shared.Dto;

namespace GenmCloud.Shared.HttpContact.Request
{
    public class UserLoginRequest : BaseRequest
    {
        public override string route { get => "api/User/Login"; }

        public LoginDto Parameter { get; set; }
    }
}
