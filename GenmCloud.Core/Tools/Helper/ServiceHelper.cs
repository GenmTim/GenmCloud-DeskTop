using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GenmCloud.Core.Tools.Helper
{
    public static class ServiceHelper
    {
        public static readonly int RequestOk = 200;

        public static string SetPassword(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            byte[] fromData = Encoding.UTF8.GetBytes(password);
            byte[] tagetData = md5.ComputeHash(fromData);

            string byte2String = null;

            for (int i = 0; i < tagetData.Length; i++)
            {
                byte2String += tagetData[i].ToString("x2");
            }
            return byte2String;
        }
    }
}
