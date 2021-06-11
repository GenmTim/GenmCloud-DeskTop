using GenmCloud.ApiService.Service;
using GenmCloud.Core.Tools.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenmCloud.Core.Manager.Impl
{
    public class NickNameManager
    {
        private static NickNameManager instance;
        private static IUserService userService;

        public static NickNameManager GetInstance()
        {
            if (instance == null)
            {
                instance = new NickNameManager();
            }
            return instance;
        }

        private readonly Dictionary<uint, string> nickNameMap;

        // TODO 多几个默认的头像选择
        private string DefaultAvatar = "无名";

        private NickNameManager()
        {
            nickNameMap = new Dictionary<uint, string>();
        }

        public void Store(uint id, string avatar)
        {
            nickNameMap[id] = avatar;

        }

        public string Get(uint id)
        {
            if (nickNameMap.ContainsKey(id))
            {
                return nickNameMap[id];
            }
            else
            {
                UpdateNickName(id);
                return DefaultAvatar;
            }
        }

        private async void UpdateNickName(uint id)
        {
            var res = await userService.GetNickName(id);
            if (res.StatusCode == ServiceHelper.RequestOk)
            {
                nickNameMap[id] = res.Result;
            }
        }
    }
}
