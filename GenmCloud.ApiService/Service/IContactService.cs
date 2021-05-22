using GenmCloud.Shared.Dto;
using GenmCloud.Shared.HttpContact;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenmCloud.ApiService.Service
{
    public interface IContactService
    {
        Task<BaseResponse<ContactQueryDto>> QueryUserToContactByEmail(string email);

        Task<BaseResponse<ContactQueryDto>> QueryUserContactStateById(uint id);

        Task<BaseResponse> RequestContact(uint id);

        Task<BaseResponse> AssentRequestContact(uint id);

        Task<BaseResponse<List<ContactDto>>> GetAllContact();


    }
}
