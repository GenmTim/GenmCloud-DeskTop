using GenmCloud.Shared.Common.Conf;
using GenmCloud.Shared.Common.Session;
using GenmCloud.Shared.Dto;
using GenmCloud.Shared.HttpContact;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GenmCloud.ApiService.Service.Impl
{
    public class DownloadService : IDownloadService
    {
        public async Task<BaseResponse<FileDto>> Prepare(uint fileID)
        {
            return await new BaseServiceRequest()
            .GetRequest<BaseResponse<FileDto>>
            (string.Format("file/meta?file_id={0}", fileID), null, Method.GET);
        }

        public void DownloadFile(uint fileID, long from, long to, Action<Stream> callback)
        {
            var client = new RestClient(Conf.ServerUrl);
            var request = new RestRequest(string.Format("file/download?file_id={0}", fileID), Method.GET);
            request.AddHeader(SessionService.TokenHeaderName, SessionService.Token);
            request.AddHeader("Range", string.Format("bytes={0}-{1}", from, to));
            request.ResponseWriter = responseStream =>
            {
                using (responseStream)
                {
                    callback(responseStream);
                }
            };
            client.DownloadData(request);
        }
    }
}
