using GenmCloud.ApiService.Service.Impl;
using GenmCloud.Shared.Common.Session;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApiServiceTestProject
{
    [TestClass]
    public class UnitTest
    {
        public static string Token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6MywiZXhwIjoxNjIzMzI4MzQxLCJpYXQiOjE2MjI3MjM1NDEsImlzcyI6Imdlbm0iLCJzdWIiOiJ1c2VyIHRva2VuIn0.orpPQWoGykMGAvQCXuSzl51HrQqjfTdR9jr7CWYmlvM";

        [TestMethod]
        public void TestUpload()
        {
            SessionService.Token = Token;
            var service = new FileService();
            var res = service.Upload(3, @"Z:\tmp\TestFile.txt");
            res.Wait();
            Assert.IsTrue(res.Result.StatusCode == 200);
        }
    }
}
