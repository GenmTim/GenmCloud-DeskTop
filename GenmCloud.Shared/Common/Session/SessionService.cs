using GenmCloud.Shared.Dto;

namespace GenmCloud.Shared.Common.Session
{
    public static class SessionService
    {
        public static string Token;
        public static string TokenHeaderName = "Authorization";

        public static UserDto User;

        public static int RequestOK = 200;
    }
}
