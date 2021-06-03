using GenmCloud.Shared.DataInterfaces;
using Newtonsoft.Json;

namespace GenmCloud.Shared.Dto
{
    public class BaseDto : ViewModelBase
    {
        [JsonProperty("id")]
        public uint ID { get; set; }
    }

    public class TokenDto
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
