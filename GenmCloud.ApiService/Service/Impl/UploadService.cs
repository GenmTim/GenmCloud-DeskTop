using GenmCloud.Shared.Common.Session;
using GenmCloud.Shared.HttpContact;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GenmCloud.ApiService.Service.Impl
{

    public class FragmentPolicy
    {
        [JsonProperty("size")]
        public Int32 FragmentSize;
    }

    public class UploadPrepareDto
    {
        [JsonProperty("token")]
        public string Token;

        [JsonProperty("policy")]
        public FragmentPolicy Policy;
    }

    public class FragmentInfo
    {
        public int Index;
        public long Size;
    }

    public class UploadService : IUploadService
    {
        public async Task<BaseResponse> Upload(uint folderId, string filePath)
        {
            var request = new RestRequest();
            request.AddFile("file", filePath);
            var restClient = new RestClient { BaseUrl = new Uri(String.Format("http://localhost:1026/api/v1/file?folder={0}", folderId)) };
            restClient.AddDefaultHeader("Authorization", SessionService.Token);

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

        public async Task<BaseResponse> Prepare()
        {
            return await new BaseServiceRequest()
               .GetRequest<BaseResponse>
               (string.Format("file/upload/prepare"), null, Method.GET);
        }

        public async Task<BaseResponse> UploadFragment(byte[] buf, string token, FragmentInfo info)
        {
            var request = new RestRequest();
            request.AddFile("file", buf, token + info.Index);

            var restClient = new RestClient { BaseUrl = new Uri(String.Format("http://localhost:1026/api/v1/file/upload/fragment?uploadToken={0}", token)) };
            restClient.AddDefaultHeader("Authorization", SessionService.Token);

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

        public async Task<BaseResponse> Complete(string token)
        {
            return await new BaseServiceRequest()
               .GetRequest<BaseResponse>
               (string.Format("file/upload/complete?uploadToken={0}", token), null, Method.GET);
        }

        public Task<BaseResponse> Cancel(string token)
        {
            throw new NotImplementedException();
        }
    }
}
