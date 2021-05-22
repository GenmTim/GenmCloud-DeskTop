using Newtonsoft.Json;

namespace GenmCloud.Shared.Dto
{
    public class LoginDto
    {
        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }
    }
}
