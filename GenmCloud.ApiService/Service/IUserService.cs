using GenmCloud.Shared.Dto;
using GenmCloud.Shared.HttpContact;
using System.Threading.Tasks;

namespace GenmCloud.ApiService.Service
{
    public interface IUserService
    {
        Task<BaseResponse<TokenDto>> LoginAsync(string username, string passWord);


        Task<BaseResponse<TokenDto>> RegisterAsync(string username, string passWord);

        Task<BaseResponse<UserDto>> GetUserInfo(uint id);
    }
}
