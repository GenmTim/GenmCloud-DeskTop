using GenmCloud.ApiService.Service.Impl;
using GenmCloud.Core.Tools.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenmCloud.Core.Manager.Impl
{
    public static class UploadConfig
    {
        // 分片大小
        public static long FragmentSize = 4 * 1024 * 1024;

        // 每次读取的大小
        public static int ReadSize = 8 * 1024;

        // 读取完指定大小，就进行页面显示
        public static long ShowSize = 256 * 1024;
    }

    public class UploadManager
    {
        private UploadManager()
        {

        }

        private static UploadManager instance;
        private UploadService uploadService = new UploadService();

        public static UploadManager GetInstance()
        {
            if (instance == null)
            {
                instance = new UploadManager();
            }
            return instance;
        }

        private Semaphore semaphore = new Semaphore(2, 2);

        public void RunUpload(byte[] buf, FragmentInfo fragmentInfo, Action<bool> f)
        {
            semaphore.WaitOne();
            Upload(buf, fragmentInfo, f);
        }

        public async void Upload(byte[] buf, FragmentInfo fragmentInfo, Action<bool> f)
        {
            var res = await uploadService.UploadFragment(buf, fragmentInfo.Token, fragmentInfo);
            if (res.StatusCode == ServiceHelper.RequestOk)
            {
                f(true);
            }
            else
            {
                f(false);
            }
            semaphore.Release();
        }
    }
}
