using GenmCloud.Shared.DataInterfaces;
using GenmCloud.Shared.Dto;
using GenmCloud.Shared.HttpContact;
using GenmCloud.Shared.HttpContact.Request;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GenmCloud.ApiService
{
    public class UserService : BaseService<UserDto>, IUserRepository
    {
        public async Task<BaseResponse<UserInfoDto>> LoginAsync(string account, string passWord)
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse<UserInfoDto>>(new UserLoginRequest()
            {
                Parameter = new LoginDto() { Account = account, PassWord = passWord }
            }, Method.POST);
        }
    }
}
