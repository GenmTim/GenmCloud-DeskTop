using GenmCloud.Service;
using GenmCloud.Shared.Common.Session;
using GenmCloud.Shared.DataInterfaces;
using GenmCloud.Shared.Dto;
using GenmCloud.Shared.HttpContact;
using GenmCloud.Shared.HttpContact.Request;
using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;

namespace GenmCloud.ApiService
{

    public class UserService : BaseService<UserDto>, IUserRepository
    {
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
