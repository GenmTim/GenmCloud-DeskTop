using GenmCloud.Shared.Common.Session;
using GenmCloud.Shared.Dto;
using GenmCloud.Shared.HttpContact;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
