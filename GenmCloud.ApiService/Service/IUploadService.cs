using GenmCloud.ApiService.Service.Impl;
using GenmCloud.Shared.HttpContact;
using System.Threading.Tasks;

namespace GenmCloud.ApiService.Service
{
    public interface IUploadService
    {
        Task<BaseResponse> Upload(uint folderId, string filePath);

        Task<BaseResponse<UploadPrepareDto>> Prepare(string fileName, long fileSize, uint folderId);

        Task<BaseResponse> UploadFragment(byte[] buf, string token, FragmentInfo info);

        Task<BaseResponse> Complete(string token);

        Task<BaseResponse> Cancel(string token);
    }
}
