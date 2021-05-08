using GenmCloud.Shared.Dto;

namespace GenmCloud.Shared.HttpContact.Request
{
    public class UserLoginRequest : BaseRequest
    {
        public override string route { get => "login"; }

        public LoginDto Parameter { get; set; }
    }

    public class UserRegisterRequest : BaseRequest
    {
        public override string route { get => "signup"; }

        public LoginDto Parameter { get; set; }
    }
}
