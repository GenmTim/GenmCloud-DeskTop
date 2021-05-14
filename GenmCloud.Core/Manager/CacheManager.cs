using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenmCloud.Core.Manager
{
    // 全局唯一，懒汉式单例
    public class CacheManager
    {
        private string cacheDirPath;
        private static CacheManager instance;

        private CacheManager()
        {
            // 读取配置文件，获取缓存位置
            cacheDirPath = @"C:\Users\46777\Documents\GenmCloud";
            InitCacheFolder();
        }

        public static CacheManager GetInstance()
        {
            if (instance == null)
            {
                instance = new CacheManager();
            }
            return instance;
        }

        private void InitCacheFolder()
        {
            CreateCacheRootFolder();
            CreateChatCacheFile();
        }

        public void CreateCacheRootFolder()
        {
            // 判断是否存在缓存目录，不存在则创建
            if (!Directory.Exists(cacheDirPath))
            {
                Directory.CreateDirectory(cacheDirPath);
            }
        }

        // 实现参考 【如何创建文件或文件夹（C# 编程指南）】https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/file-system/how-to-create-a-file-or-folder
        // 创建聊天记录缓存文件
        public void CreateChatCacheFile()
        {
            string chatCachePath = cacheDirPath + @"\chat";
            if (!File.Exists(chatCachePath))
            {
                using (FileStream fs = File.Create(chatCachePath))
                {
                }
            }
        }

        public async void SaveRecord(string cacheName, string cacheValue)
        {
            //TODO 记录隐私安全性，存取性能
            string cachePath = cacheDirPath + @"\" + cacheName;
            using (StreamWriter file = new StreamWriter(cachePath, append: true))
            {
                await file.WriteLineAsync(cacheValue);
            }
        }
    }
}
