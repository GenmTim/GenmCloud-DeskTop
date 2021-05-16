using GenmCloud.Shared.DataInterfaces;
using Newtonsoft.Json;

namespace GenmCloud.Shared.Dto
{
    public class BaseDto : ViewModelBase
    {
        public uint Id { get; set; }
    }

    public class TokenDto
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
