using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenmCloud.Shared.Dto
{
    public class ChatDto
    {
        [JsonProperty("id")]
        public uint Id;

        [JsonProperty("type")]
        public string Type;

        [JsonProperty("subType")]
        public string SubType;

        [JsonProperty("senderId")]
        public uint SenderId;
        
        [JsonProperty("receiverId")]
        public uint ReceiverId;
        
        [JsonProperty("isRead")]
        public uint IsRead;
        
        [JsonProperty("data")]
        public string Data;

        [JsonProperty("token")]
        public uint Token;
    }
}
