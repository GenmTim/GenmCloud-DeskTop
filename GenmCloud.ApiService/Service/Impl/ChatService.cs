using GenmCloud.Shared.Common.Session;
using GenmCloud.Shared.Dto;
using GenmCloud.Shared.HttpContact;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenmCloud.ApiService.Service.Impl
{
    public class ChatService : IChatService
    {
        public async Task<BaseResponse<List<ChatMsgDto>>> GetChatMsgHistoryBySingleObj(uint UserId)
        {
            return await new BaseServiceRequest()
                .GetRequest<BaseResponse<List<ChatMsgDto>>>
                (string.Format("chat/history?user_id={0}", UserId), null, Method.GET);
        }

        public async Task<BaseResponse<ChatObjDto>> GetChatObj(uint id)
        {
            return await new BaseServiceRequest()
                .GetRequest<BaseResponse<ChatObjDto>>
                (string.Format("chat/obj/{0}", id), null, Method.GET);
        }

        public async Task<BaseResponse<List<uint>>> GetChatObjIdList()
        {
            return await new BaseServiceRequest()
                .GetRequest<BaseResponse<List<uint>>>
                (string.Format("chat/obj/ids"), null, Method.GET);
        }

        public async Task<BaseResponse<ChatMsgDto>> GetLastChatMsgByObjId(uint id)
        {
            return await new BaseServiceRequest()
                .GetRequest<BaseResponse<ChatMsgDto>>
                (string.Format("chat/obj/{0}/lastMsg", id), null, Method.GET);
        }
    }
}
