using GenmCloud.Shared.Common.Session;
using GenmCloud.Shared.Dto;
using GenmCloud.Shared.HttpContact;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenmCloud.ApiService.Service.Impl
{
    internal struct NewFolder
    {
        [JsonProperty("parent_id")]
        public uint ParentID { get; set; }

        [JsonProperty("folder_name")]
        public string FolderName { get; set; }
    }


    public class FolderService : IFolderService
    {
        public async Task<BaseResponse<List<FolderListDto>>> GetForlderList()
        {
            return await new BaseServiceRequest()
            .GetRequest<BaseResponse<List<FolderListDto>>>
            (string.Format("folder/list"), null, Method.GET);
        }

        public async Task<BaseResponse<List<FileDto>>> GetFileListByFolder(uint id)
        {
            return await new BaseServiceRequest()
            .GetRequest<BaseResponse<List<FileDto>>>
            (string.Format("folder/{0}/list", id), null, Method.GET);
        }

        public async Task<BaseResponse> CreateFolder(uint parentFolderId, string newFolderName)
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse>("folder", 
                new NewFolder { ParentID=parentFolderId, FolderName=newFolderName}, Method.PUT);
        }
    }
}
