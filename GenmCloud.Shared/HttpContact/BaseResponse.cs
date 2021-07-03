using Newtonsoft.Json;

namespace GenmCloud.Shared.HttpContact
{
    public class BaseResponse
    {
        /// <summary>
        /// 后台消息
        /// </summary>
        [JsonProperty("msg")]
        public string Message { get; set; }

        /// <summary>
        /// 返回状态
        /// </summary>
        [JsonProperty("code")]
        public int StatusCode { get; set; }

        [JsonProperty("data")]
        public object Result { get; set; }
    }

    public class BaseResponse<T> : BaseResponse
    {
        ///// <summary>
        ///// 后台消息
        ///// </summary>
        //[JsonProperty("msg")]
        //public string Message { get; set; }

        ///// <summary>
        ///// 返回状态
        ///// </summary>
        //[JsonProperty("code")]
        //public int StatusCode { get; set; }

        [JsonProperty("data")]
        public new T Result { get; set; }
    }
}

