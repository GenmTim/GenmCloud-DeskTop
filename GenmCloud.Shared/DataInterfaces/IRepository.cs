/*
*
* 文件名    ：IConsumptionService                             
* 程序说明  : 系统接口管理层
* 
*/

namespace GenmCloud.Shared.DataInterfaces
{
    using GenmCloud.Shared.Common.Collections;
    using GenmCloud.Shared.Common.Query;
    using GenmCloud.Shared.Dto;
    using GenmCloud.Shared.HttpContact;
    using System.Threading.Tasks;

    public interface IRepository<T>
    {
        Task<BaseResponse<PagedList<T>>> GetAllListAsync(QueryParameters parameters);

        Task<BaseResponse<T>> GetAsync(int id);

        Task<BaseResponse> SaveAsync(T model);

        Task<BaseResponse> AddAsync(T model);

        Task<BaseResponse> DeleteAsync(int id);

        Task<BaseResponse> UpdateAsync(T model);
    }

    public interface IUserRepository : IRepository<UserDto>
    {
        Task<BaseResponse<UserInfoDto>> LoginAsync(string account, string passWord);
    }
}
