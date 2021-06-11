using GenmCloud.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenmCloud.Core.Manager.Impl
{
    public class UserInfoManager
    {
        private static UserInfoManager instance;
        public static UserInfoManager GetInstance()
        {
            if (instance == null)
            {
                instance = new UserInfoManager();
            }
            return instance;
        }

        private readonly Dictionary<uint, UserDto> userInfoMap;

        // TODO 多几个默认的头像选择
        private string DefaultAvatar = "http://localhost:1026/static/avatar/3.jpg";
        private string DefaultNickName = "无名";

        private UserInfoManager()
        {
            userInfoMap = new Dictionary<uint, UserDto>();
        }

        public void Store(uint id, UserDto userinfo)
        {
            userInfoMap[id] = userinfo;

        }

        public UserDto Get(uint id)
        {
            if (userInfoMap.ContainsKey(id))
            {
                return userInfoMap[id];
            }
            else
            {
                return new UserDto { Avatar= DefaultAvatar, NickName=DefaultNickName };
            }
        }

        public bool Has(uint id)
        {
            return userInfoMap.ContainsKey(id);
        }
    }
}
