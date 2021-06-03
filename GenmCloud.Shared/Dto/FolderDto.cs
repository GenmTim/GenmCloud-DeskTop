using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenmCloud.Shared.Dto
{
    public class FolderDto : BaseDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parent_id")]
        public uint ParentID { get; set; }

        [JsonProperty("owner_id")]
        public uint OwnerID { get; set; }
    }

    public class FolderListDto
    {
        [JsonProperty("folder")]
        public FolderDto Folder { get; set; }

        [JsonProperty("children")]
        public List<FolderListDto> Children { get; set; }
    }

}
