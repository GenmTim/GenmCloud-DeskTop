using GenmCloud.Shared.HttpContact;
using System.Threading.Tasks;

namespace GenmCloud.ApiService.Service
{
    public interface IFileService
    {
        Task<BaseResponse> Upload(string filePath);
    }
}
