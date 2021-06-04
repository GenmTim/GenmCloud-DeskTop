using GenmCloud.Shared.Dto;
using GenmCloud.Shared.HttpContact;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GenmCloud.ApiService.Service
{
    public interface IFolderService
    {
        Task<BaseResponse<List<FolderListDto>>> GetForlderList();

        Task<BaseResponse<List<FileDto>>> GetFileListByFolder(uint id);
    }
}
