using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenmCloud.Shared.Dto
{
    public class FileDto
    {
        [JsonProperty("id")]
        public uint Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("owner_name")]
        public string OwnerName { get; set; }

        [JsonProperty("lastUpdatedTime")]
        public string LastUpdatedTime { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
    }
}
