using GenmCloud.Shared.Common.Session;
using GenmCloud.Shared.Dto;
using GenmCloud.Shared.HttpContact;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenmCloud.ApiService.Service.Impl
{
    public class FolderService : IFolderService
    {
        public async Task<BaseResponse<List<FolderListDto>>> GetForlderList()
        {
            return await new BaseServiceRequest()
            .GetRequest<BaseResponse<List<FolderListDto>>>
            (string.Format("folder/list?token={0}", SessionService.Token), null, Method.GET);
        }

        public async Task<BaseResponse<List<FileDto>>> GetFileListByFolder(uint id)
        {
            return await new BaseServiceRequest()
            .GetRequest<BaseResponse<List<FileDto>>>
            (string.Format("folder/{0}/list?token={1}", id, SessionService.Token), null, Method.GET);
        }
    }
}
