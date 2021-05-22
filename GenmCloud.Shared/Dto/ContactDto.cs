using Newtonsoft.Json;
using Prism.Mvvm;

namespace GenmCloud.Shared.Dto
{
    public class ContactDto
    {
        [JsonProperty("Id")]
        public uint Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }
    }

    public class ContactQueryDto : BindableBase
    {
        [JsonProperty("id")]
        public uint Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        /// <summary>
        /// State 0 表示无任何记录 1 表示已申请  2 表示已为好友
        /// </summary>
        private int state;
        [JsonProperty("state")]
        public int State
        {
            get => state;
            set
            {
                state = value;
                RaisePropertyChanged();
            }
        }
    }
}
