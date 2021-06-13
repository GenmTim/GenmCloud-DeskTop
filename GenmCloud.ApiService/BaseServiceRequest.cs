using GenmCloud.Service;
using GenmCloud.Shared.Common;
using GenmCloud.Shared.HttpContact;
using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;

namespace GenmCloud.ApiService
{


    /// <summary>
    /// 请求服务基类
    /// </summary>
    public class BaseServiceRequest
    {
        private readonly string _requestUrl = Contract.ServerUrl;

        public string requestUrl
        {
            get { return _requestUrl; }
        }

        /// <summary>
        /// restSharp实例
        /// </summary>
        public RestSharpCertificateMethod restSharp = new RestSharpCertificateMethod();

        /// <summary>
        /// T请求
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <param name="method">方法类型</param>
        /// <returns></returns>
        public async Task<Response> GetRequest<Response>(BaseRequest request, Method method) where Response : class
        {
            string pms = request.GetPropertiesObject();
            string url = requestUrl + request.route;
            if (!string.IsNullOrWhiteSpace(request.getParameter))
                url += request.getParameter;
            Response result = await restSharp.RequestBehavior<Response>(url, method, pms);
            return result;
        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <typeparam name="Response"></typeparam>
        /// <param name="url">地址</param>
        /// <param name="pms">参数</param>
        /// <param name="method">方法类型</param>
        /// <returns></returns>
        public async Task<Response> GetRequest<Response>(string route, object obj, Method method) where Response : class
        {
            string pms = string.Empty;
            if (!string.IsNullOrWhiteSpace(obj?.ToString())) pms = JsonConvert.SerializeObject(obj);
            Response result = await restSharp.RequestBehavior<Response>(requestUrl + route, method, pms);
            return result;
        }
    }
}
