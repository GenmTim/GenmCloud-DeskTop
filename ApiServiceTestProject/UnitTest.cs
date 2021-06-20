using GenmCloud.ApiService.Service.Impl;
using GenmCloud.Shared.Common.Conf;
using GenmCloud.Shared.Common.Session;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ApiServiceTestProject
{
    [TestClass]
    public class UnitTest
    {
        public static string Token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6MywiZXhwIjoxNjI0Nzc3MjY2LCJpYXQiOjE2MjQxNzI0NjYsImlzcyI6Imdlbm0iLCJzdWIiOiJ1c2VyIHRva2VuIn0.YPInmH2oG3XUmtDGSaXGGgmGEjxLnTn2CG4qq1demFw";
        public static string BaseUrl = "http://localhost:1026/api/v1/";
        
        public UnitTest()
        {
            SessionService.Token = Token;
            Conf.ServerUrl = BaseUrl;
        }

        [TestMethod]
        public void TestUpload()
        {
            SessionService.Token = Token;
            var service = new UploadService();
            var res = service.Upload(3, @"Z:\tmp\TestFile.txt");
            res.Wait();
            Assert.IsTrue(res.Result.StatusCode == 200);
        }

        [TestMethod]
        public void TestListInsert()
        {
            List<long> list = new List<long>();
            list.Insert(0, 1);
            list.Insert(0, 2);
            list.Insert(0, 3);
            Assert.IsTrue(list[0] == 3);
            Assert.IsTrue(list[1] == 2);
            Assert.IsTrue(list[2] == 1);
        }

        [TestMethod]
        public void TestDownload()
        {
            var service = new DownloadService();
            var res = service.Prepare(63);
            res.Wait();

            var fileMeta = res.Result.Result;
            service.DownloadFile(63, (stream) =>
            {
                using var destFile = File.OpenWrite(@"Z:\tmp\" + fileMeta.Name);
                var bufSize = 4 * 1024;
                var buf = new byte[bufSize];
                while (true)
                {
                    int n = stream.Read(buf, 0, bufSize);
                    if (n == 0) return;
                    destFile.Write(buf, 0, n);
                }
            });
        }
    }
}
