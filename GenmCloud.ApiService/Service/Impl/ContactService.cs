using GenmCloud.Shared.Common.Session;
using GenmCloud.Shared.Dto;
using GenmCloud.Shared.HttpContact;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenmCloud.ApiService.Service.Impl
{
    public struct Eamil
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class ContactService : IContactService
    {
        public async Task<BaseResponse<ContactQueryDto>> QueryUserToContactByEmail(string email)
        {
            return await new BaseServiceRequest()
                .GetRequest<BaseResponse<ContactQueryDto>>
                (string.Format("contact/query/email?email={0}", email), null, Method.GET);
        }

        public async Task<BaseResponse> RequestContact(uint id)
        {
            if (id == SessionService.User.ID) return new BaseResponse { StatusCode = -1 };
            return await new BaseServiceRequest()
                .GetRequest<BaseResponse>
                (string.Format("contact/request?contact_id={0}", id), null, Method.PUT);
        }

        public async Task<BaseResponse<List<ContactDto>>> GetAllContact()
        {
            return await new BaseServiceRequest()
                .GetRequest<BaseResponse<List<ContactDto>>>
                (string.Format("contact/list"), null, Method.GET);
        }

        public async Task<BaseResponse<ContactQueryDto>> QueryUserContactStateById(uint id)
        {
            return await new BaseServiceRequest()
                .GetRequest<BaseResponse<ContactQueryDto>>
                (string.Format("contact/query/id?id={0}", id), null, Method.GET);
        }

        public async Task<BaseResponse> AssentRequestContact(uint id)
        {
            return await new BaseServiceRequest()
                .GetRequest<BaseResponse>
                (string.Format("contact/request/assent/{0}", id), null, Method.POST);
        }
    }
}
