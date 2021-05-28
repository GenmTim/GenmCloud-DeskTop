using System.Collections.Generic;

namespace GenmCloud.Core.Manager
{
    public class AvatarManager : IAvatarManager
    {
        private static AvatarManager instance;
        public static AvatarManager GetInstance()
        {
            if (instance == null)
            {
                instance = new AvatarManager();
            }
            return instance;
        }

        private readonly Dictionary<uint, string> avatarMap;

        // TODO 多几个默认的头像选择
        private string DefaultAvatar = "http://localhost:1026/static/avatar/3.jpg";

        private AvatarManager()
        {
            avatarMap = new Dictionary<uint, string>();
        }

        public void Store(uint id, string avatar)
        {
            avatarMap[id] = avatar;

        }

        public string Get(uint id)
        {
            if (avatarMap.ContainsKey(id))
            {
                return avatarMap[id];
            }
            else
            {
                return DefaultAvatar;
            }
        }
    }
}
