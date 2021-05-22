using GenmCloud.Core.Data.VO;
using GenmCloud.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenmCloud.Chat.Manager
{
    interface IChatMsgManager
    {
        /// <summary>
        /// 获取联系对象列表
        /// </summary>
        /// <returns></returns>
        ObservableCollection<ChatObjVO> GetChatObjList();

        /// <summary>
        /// 获取联系对象的联系记录列表
        /// </summary>
        /// <param name="chatObjVO"></param>
        /// <returns></returns>
        ObservableCollection<ChatMsgVO> GetChatMsgListByObj(ChatObjVO chatObjVO);

        /// <summary>
        /// 在展示层，包装好要传输的消息，在此进行发送操作。
        /// </summary>
        /// <param name="chatMsgDto"></param>
        void SendMessage(ChatMsgDto chatMsgDto);
    }
}
