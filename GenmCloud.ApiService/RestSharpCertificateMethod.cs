/*
* 文件名称  : RestSharpCertificateMethod.cs                       
* 程序说明  : 请求服务基类
* 
*/

namespace GenmCloud.Service
{
    using GenmCloud.Shared.Common.Session;
    using GenmCloud.Shared.HttpContact;
    using Newtonsoft.Json;
    using RestSharp;
    using System.Threading.Tasks;

    /// <summary>
    /// RestSharp Client
    /// </summary>
    public class RestSharpCertificateMethod
    {
        /// <summary>
        /// 请求数据
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="method">请求类型</param>
        /// <param name="pms">参数</param>
        /// <param name="isToken">是否Token</param>
        /// <param name="isJson">是否Json</param>
        /// <returns></returns>
        //[GlobalLoger]
        public async Task<Response> RequestBehavior<Response>(string url, Method method, string pms,
            bool isToken = true, bool isJson = true) where Response : class
        {
            RestClient client = new RestClient(url);
            RestRequest request = new RestRequest(method);
            if (isToken)
            {
                if (!string.IsNullOrEmpty(SessionService.Token))
                {
                    client.AddDefaultHeader("Authorization", SessionService.Token);
                }
            }
            switch (method)
            {
                case Method.GET:
                    request.AddHeader("Content-Type", "application/json");
                    break;
                case Method.POST:
                    if (isJson)
                    {
                        request.AddHeader("Content-Type", "application/json");
                        request.AddJsonBody(pms);
                    }
                    else
                    {
                        request.AddHeader("Content-Type", "application/json");
                        request.AddParameter("application/x-www-form-urlencoded",
                            pms, ParameterType.RequestBody);
                    }
                    break;
                case Method.PUT:
                    if (isJson)
                    {
                        request.AddHeader("Content-Type", "application/json");
                        request.AddJsonBody(pms);
                    }
                    break;
                case Method.DELETE:
                    request.AddHeader("Content-Type", "application/json");
                    break;
                default:
                    request.AddHeader("Content-Type", "application/json");
                    break;
            }
            var response = await client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<Response>(response.Content);
            }
            else
            {
                return new BaseResponse()
                {
                    StatusCode = (int)response.StatusCode,
                    Message = response.StatusDescription ?? response.ErrorMessage
                } as Response;
            }
        }
    }
}
