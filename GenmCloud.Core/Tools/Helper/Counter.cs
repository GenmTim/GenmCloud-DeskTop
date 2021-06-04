using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenmCloud.Core.Tools.Helper
{
    public static class Counter
    {
        private static uint UploadTaskID = 0;
        public static uint NextUploadTaskID()
        {
            return UploadTaskID++;
        }
    }
}
