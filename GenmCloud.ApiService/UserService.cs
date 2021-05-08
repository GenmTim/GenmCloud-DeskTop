using GenmCloud.Shared.DataInterfaces;
using GenmCloud.Shared.Dto;
using GenmCloud.Shared.HttpContact;
using GenmCloud.Shared.HttpContact.Request;
using RestSharp;
using System.Threading.Tasks;

namespace GenmCloud.ApiService
{
    public class UserService : BaseService<UserDto>, IUserRepository
    {
        public async Task<BaseResponse> LoginAsync(string username, string passWord)
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse>(new UserLoginRequest()
            {
                Parameter = new LoginDto() { Username = username, Password = passWord }
            }, Method.POST);
        }

        public async Task<BaseResponse> RegisterAsync(string username, string passWord)
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse>(new UserRegisterRequest()
            {
                Parameter = new LoginDto() { Username = username, Password = passWord }
            }, Method.POST);
        }
    }
}
