using GenmCloud.Shared.Dto;
using GenmCloud.Shared.HttpContact;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenmCloud.ApiService.Service
{
    public interface IFolderService
    {
        Task<BaseResponse<List<FolderListDto>>> GetForlderList();

        Task<BaseResponse<List<FileDto>>> GetFileListByFolder(uint id);

        Task<BaseResponse> CreateFolder(uint parentFolderId, string newFolderName);
    }
}
