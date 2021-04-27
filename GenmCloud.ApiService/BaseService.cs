﻿namespace GenmCloud.ApiService
{
    using GenmCloud.Shared.Common.Collections;
    using GenmCloud.Shared.Common.Query;
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
            var r = await new BaseServiceRequest().
             GetRequest<BaseResponse>($@"api/{servicesName}", model, Method.POST);
            return r;
        }

        public async Task<BaseResponse> DeleteAsync(int id)
        {
            var r = await new BaseServiceRequest().
              GetRequest<BaseResponse>($@"api/{servicesName}?id={id}", string.Empty, Method.DELETE);
            return r;
        }

        public async Task<BaseResponse<PagedList<T>>> GetAllListAsync(QueryParameters pms)
        {
            var r = await new BaseServiceRequest().
               GetRequest<BaseResponse<PagedList<T>>>($@"api/{servicesName}/GetAll?PageIndex={pms.PageIndex}&PageSize={pms.PageSize}&Search={pms.Search}", string.Empty, Method.GET);
            return r;
        }

        public async Task<BaseResponse<T>> GetAsync(int id)
        {
            var r = await new BaseServiceRequest().
               GetRequest<BaseResponse<T>>($@"api/{servicesName}?id={id}", string.Empty, Method.GET);
            return r;
        }

        public async Task<BaseResponse> SaveAsync(T model)
        {
            var r = await new BaseServiceRequest().
                GetRequest<BaseResponse>($@"api/{servicesName}", model, Method.POST);
            return r;
        }

        public async Task<BaseResponse> UpdateAsync(T model)
        {
            var r = await new BaseServiceRequest().
                GetRequest<BaseResponse>($@"api/{servicesName}", model, Method.PUT);
            return r;
        }
    }
}
