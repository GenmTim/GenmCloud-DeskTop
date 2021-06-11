using GenmCloud.Shared.Common.Session;
using GenmCloud.Shared.Dto;
using GenmCloud.Shared.HttpContact;
using GenmCloud.Shared.HttpContact.Request;
using RestSharp;
using System.Threading.Tasks;

namespace GenmCloud.ApiService.Service.Impl
{

    public class UserService : BaseService<UserDto>, IUserService
    {
        public async Task<BaseResponse<string>> GetAvatar(uint id)
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse<string>>(string.Format("/user/avatar/{0}", 3), null, Method.POST);
        }

        public async Task<BaseResponse<string>> GetNickName(uint id)
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse<string>>(string.Format("/user/name/{0}", 3), null, Method.POST);
        }

        public async Task<BaseResponse<UserDto>> GetUserInfo(uint id)
        {
            if (id == 0)
            {
                return await new BaseServiceRequest().GetRequest<BaseResponse<UserDto>>(string.Format("user"), null, Method.GET);
            }
            else
            {
                return await new BaseServiceRequest().GetRequest<BaseResponse<UserDto>>(string.Format("user/{0}", id), null, Method.GET);
            }
        }

        public async Task<BaseResponse<TokenDto>> LoginAsync(string username, string passWord)
        {
            var res = await new BaseServiceRequest().GetRequest<BaseResponse<TokenDto>>(new UserLoginRequest()
            {
                Parameter = new LoginDto() { Username = username, Password = passWord }
            }, Method.POST);
            SessionService.Token = "Bearer " + res?.Result?.Token;
            return res;
        }

        public async Task<BaseResponse<TokenDto>> RegisterAsync(string username, string passWord)
        {
            var res = await new BaseServiceRequest().GetRequest<BaseResponse<TokenDto>>(new UserRegisterRequest()
            {
                Parameter = new LoginDto() { Username = username, Password = passWord }
            }, Method.POST);
            SessionService.Token = "Bearer " + res?.Result?.Token;
            return res;
        }
    }
}
