using System;
using System.Collections.Generic;
using System.Text;

namespace GenmCloud.Shared.HttpContact
{
    public class BaseResponse
    {
        /// <summary>
        /// 后台消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回状态
        /// </summary>
        public int StatusCode { get; set; }

        public object Result { get; set; }
    }

    public class BaseResponse<T>
    {
        /// <summary>
        /// 后台消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回状态
        /// </summary>
        public string StatusCode { get; set; }

        public T Result { get; set; }
    }
}

