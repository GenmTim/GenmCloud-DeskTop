using GenmCloud.Shared.Common.Session;
using GenmCloud.Shared.HttpContact;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace GenmCloud.ApiService.Service.Impl
{
    public class FileService : IFileService
    {
        public async Task<BaseResponse> Upload(uint folderId, string filePath)
        {
            var request = new RestRequest();
            request.AddFile("file", filePath);
            var restClient = new RestClient { BaseUrl = new Uri(String.Format("http://localhost:1026/api/v1/file?token={0}&folder={1}", SessionService.Token, folderId)) };
            var result = await restClient.ExecuteAsync(request, Method.PUT);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<BaseResponse>(result.Content);
            }
            else
            {
                return new BaseResponse()
                {
                    StatusCode = -1,
                    Message = "上传失败"
                };
            }
        }
    }
}
