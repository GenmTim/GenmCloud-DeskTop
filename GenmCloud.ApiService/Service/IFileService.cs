using GenmCloud.Shared.HttpContact;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GenmCloud.ApiService.Service
{
    public interface IFileService
    {
        public Task<BaseResponse> DeleteFile(long id);
    }
}
