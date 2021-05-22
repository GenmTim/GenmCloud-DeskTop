using Newtonsoft.Json;

namespace GenmCloud.Shared.Dto
{
    public class ChatMsgDto
    {
        [JsonProperty("id")]
        public uint Id { get; set; }

        [JsonProperty("type")]
        public uint Type { get; set; }

        [JsonProperty("subType")]
        public uint SubType { get; set; }

        [JsonProperty("senderId")]
        public uint SenderId { get; set; }

        [JsonProperty("receiverId")]
        public uint ReceiverId { get; set; }

        [JsonProperty("isRead")]
        public uint IsRead { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("token")]
        public uint Token { get; set; }
    }

    public class ChatObjDto
    {
        [JsonProperty("userId")]
        public uint UserId { get; set; }

        [JsonProperty("nickname")]
        public string Nick { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("lastMsg")]
        public ChatMsgDto LastMsg { get; set; }

        [JsonProperty("sendTime")]
        public string SendTime { get; set; }
    }
}
