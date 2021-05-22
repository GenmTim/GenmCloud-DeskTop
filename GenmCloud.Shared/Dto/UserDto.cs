using Newtonsoft.Json;
using System.ComponentModel;

namespace GenmCloud.Shared.Dto
{
    public class UserDto : BaseDto
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [Description("昵称")]
        [JsonProperty("nickname")]
        public string NickName { get; set; }


        /// <summary>
        /// 邮箱
        /// </summary>
        [Description("邮箱")]
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
    }
}
