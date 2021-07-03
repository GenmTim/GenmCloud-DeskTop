using GenmCloud.Shared.HttpContact;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GenmCloud.ApiService.Service.Impl
{
    public class FileService : IFileService
    {
        public async Task<BaseResponse> DeleteFile(long id)
        {
            await Task.Delay(1);
            return new BaseResponse();
        }
    }
}
