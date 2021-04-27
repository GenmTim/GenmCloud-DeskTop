using System;

namespace GenmCloud.ApiService
{
    using GenmCloud.Shared.HttpContact;
    using RestSharp;
    using System.Threading.Tasks;

    public class BaseService<T>
    {
        private readonly string servicesName;
        public BaseService()
        {
            servicesName = typeof(T).Name.Replace("Dto", string.Empty);
        }

        public async Task<BaseResponse> AddAsync(T model)
        {
            //var r = await new BaseServiceRequest().
            // GetRequest<BaseResponse>($@"api/{servicesName}/Add", model, Method.POST);
            //return r;
            return null;
        }
    }
}
