using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenmCloud.Storage.Data.Task
{
    public class UploadFileTask
    {
        public uint ID;
        public string FilePath;
        public string FileName;
        public long FileSize;
        public uint FolderId;
        public long FragmentNum;
        public long FileOffset;
        public Semaphore Sem;
        public string Token;
    }

    public class DownloadFileTask
    {
        public uint FileID;
        public string TargetAddr;
        public long FileSize;
        public long FileOffset;
        public string FileName;
        public FileStream TargetStream;
    }
}
