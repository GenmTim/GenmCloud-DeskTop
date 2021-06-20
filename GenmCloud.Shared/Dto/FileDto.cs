using Newtonsoft.Json;

namespace GenmCloud.Shared.Dto
{
    public class FileDto
    {
        [JsonProperty("id")]
        public uint Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("owner_name")]
        public string OwnerName { get; set; }

        [JsonProperty("lastUpdatedTime")]
        public string LastUpdatedTime { get; set; }

        [JsonProperty("thumb_addr")]
        public string ThumbAddr { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
    }

    public class FileUploadPrepareInfo
    {
        [JsonProperty("file_size")]
        public long FileSize { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("folder_id")]
        public uint OwnerFolderId { get; set; }
    }
}
