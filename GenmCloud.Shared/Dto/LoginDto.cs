using Newtonsoft.Json;

namespace GenmCloud.Shared.Dto
{
    public class LoginDto
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
