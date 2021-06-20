using GenmCloud.Shared.Common;
using GenmCloud.Shared.Common.Conf;
using GenmCloud.Shared.Common.Session;
using GenmCloud.Shared.Dto;
using GenmCloud.Shared.HttpContact;
using Newtonsoft.Json;
using RestSharp;
using System;
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
        public string Token;
    }

    public class UploadService : IUploadService
    {
        public async Task<BaseResponse> Upload(uint folderId, string filePath)
        {
            var request = new RestRequest();
            request.AddFile("file", filePath);
            var restClient = new RestClient { BaseUrl = new Uri(string.Format(Conf.ServerUrl + "file?folder={0}", folderId)) };
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

        public async Task<BaseResponse<UploadPrepareDto>> Prepare(string fileName, long fileSize, uint folderId)
        {
            var prepareInfoDto = new FileUploadPrepareInfo
            {
                FileName = fileName,
                FileSize = fileSize,
                OwnerFolderId = folderId,
            };

            return await new BaseServiceRequest()
               .GetRequest<BaseResponse<UploadPrepareDto>>
               (string.Format("file/upload/prepare"), prepareInfoDto, Method.POST);
        }

        public async Task<BaseResponse> UploadFragment(byte[] buf, string token, FragmentInfo info)
        {
            var request = new RestRequest(string.Format("file/upload/fragment?upload_token={0}", token));
            request.AddFile("file", buf, "" + info.Index);

            var restClient = new RestClient { BaseUrl = new Uri(Conf.ServerUrl) };
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
               (string.Format("file/upload/complete?upload_token={0}", token), null, Method.GET);
        }

        public Task<BaseResponse> Cancel(string token)
        {
            throw new NotImplementedException();
        }
    }
}
