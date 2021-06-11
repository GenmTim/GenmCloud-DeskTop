using GenmCloud.Shared.Dto;
using GenmCloud.Shared.HttpContact;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenmCloud.ApiService.Service
{
    public interface IChatService
    {
        /// <summary>
        /// 获取单个对象所有的聊天记录
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<ChatMsgDto>>> GetChatMsgHistoryBySingleObj(uint UserId);

        /// <summary>
        ///  获取最近所有的联系对象ID
        /// </summary>
        /// <returns></returns>
        Task<BaseResponse<List<uint>>> GetChatObjIdList();

        /// <summary>
        /// 获取最后一个消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<ChatMsgDto>> GetLastChatMsgByObjId(uint id);


        /// <summary>
        /// 获取指定联系对象信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<ChatObjDto<T>>> GetChatObj<T>(uint id);
    }
}
