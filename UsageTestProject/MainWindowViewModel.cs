using GenmCloud.ApiService.Service.Impl;
using GenmCloud.Core.Data.VO;
using GenmCloud.Core.Event;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Shared.Common;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace UsageTestProject
{
    // 需要一个生产者和消费者队列，规定数量，当超过规定的数字，则进行阻塞
    // 通过条件变量进行通知

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
            var res = await uploadService.UploadFragment(buf, "token", fragmentInfo);
            if (res.StatusCode == ServiceHelper.RequestOk)
            {
                f(true);
            } else
            {
                f(false);
            }
            semaphore.Release();
        }
    }


    class MainWindowViewModel : BindableBase
    {
        private int UploadTaskSize;

        private long uploadRate;
        public long UploadRate
        {
            get => uploadRate;
            set
            {
                uploadRate = value;
                RaisePropertyChanged();
            }
        }

        // 收集1s的传输量
        private long SecondUploaded;

        private object mtx = new object();

        private DispatcherTimer _timer = new DispatcherTimer();

        public ObservableCollection<long> UploadedList { get; set; } = new ObservableCollection<long>();

        public ObservableCollection<UploadFileItemVO> UploadList { get; set; } = new ObservableCollection<UploadFileItemVO>();

        private readonly UploadService uploadService;

        public DelegateCommand ClickCmd { get; private set; }
        public DelegateCommand<object> PauseCmd { get; private set; }
        public DelegateCommand<object> StartCmd { get; private set; }
        public DelegateCommand<object> SingleTaskCmd { get; private set; }

        public MainWindowViewModel()
        {
            uploadService = new UploadService();
            ClickCmd = new DelegateCommand(() => {
                NewUploadTask(@"Z:\iso\CentOS-8.3.2011-x86_64-dvd1.iso");
                NewUploadTask(@"Z:\iso\cn_windows_10_consumer_editions_version_1909_updated_jan_2020_x86_dvd_9c50652f.iso");
                NewUploadTask(@"Z:\iso\Windows XP SP3.iso");
                NewUploadTask(@"D:\Tmp\tmp\BigWork.zip");
            });

            StartCmd = new DelegateCommand<object>((obj) => {
                StartUpload(obj);
            });

            PauseCmd = new DelegateCommand<object>((obj) => {
                PauseUpload(obj);
            });

            SingleTaskCmd = new DelegateCommand<object>((obj) =>
            {
                if (obj is UploadFileItemVO vo)
                {
                    if (vo.State == UploadFileState.Uploading)
                    {
                        PauseUpload(vo);
                    } 
                    else
                    {
                        StartUpload(vo);
                    }
                }
            });

            _timer.Interval = new TimeSpan(0, 0, 1);   //间隔1秒
            _timer.Tick += new EventHandler((s, e) => {
                lock(mtx)
                {
                    UploadRate = SecondUploaded;
                    SecondUploaded = 0;
                }
            });
        }

        private void StartUpload(object obj)
        {
            if (obj == null)
            {
                _timer.Start();
                foreach (var vo in UploadList)
                {
                    vo.State = UploadFileState.Uploading;
                    StartUploadTask(vo);
                }
            }
            else if (obj is UploadFileItemVO vo)
            {
                vo.State = UploadFileState.Uploading;
                StartUploadTask(vo);
            }
        }

        private void PauseUpload(object obj)
        {
            if (obj == null)
            {
                _timer.Stop();
                foreach (var vo in UploadList)
                {
                    vo.State = UploadFileState.Pause;
                }
            }
            else if (obj is UploadFileItemVO vo)
            {
                vo.State = UploadFileState.Pause;
            }
        }

        private void NewUploadTask(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            // 添加UI项到列表中，并维护上传任务
            UploadFileTask task = new UploadFileTask();
            task.FragmentNum = (fileInfo.Length + UploadConfig.FragmentSize - 1) / UploadConfig.FragmentSize;
            task.FilePath = filePath;

            var vo = new UploadFileItemVO { FullName = fileInfo.FullName, Name = fileInfo.Name, Size = fileInfo.Length, Task=task };

            UploadList.Add(vo);

            // 增量当前任务数
            UploadTaskSize += 1;

            // 如果当前为第一个任务，则开启定时速率计算任务
            if (!_timer.IsEnabled)
            {
                _timer.Start();
            }

            StartUploadTask(vo);
        }

        private async void StartUploadTask(UploadFileItemVO vo)
        {
            await Task.Run(async () => {
                var res = await uploadService.Prepare();
                if (res.StatusCode != ServiceHelper.RequestOk)
                {
                    return;
                }

                var task = vo.Task;
                using (FileStream stream = File.Open(task.FilePath, FileMode.Open))
                {
                    long pos = 0;
                    var readSum = task.FileOffset;
                    stream.Position = task.FileOffset;
                    byte[] buf = new byte[UploadConfig.FragmentSize];

                    for (int index = 0; vo.State == UploadFileState.Uploading && stream.Position != stream.Length; ++index)
                    {
                        int readAmount = 0;
                        for (; readAmount < UploadConfig.FragmentSize && stream.Position != stream.Length;)
                        {
                            int readn = await stream.ReadAsync(buf.AsMemory(readAmount, UploadConfig.ReadSize));
                            readAmount += readn;
                            readSum += readn;

                            // 仅当为showSize的倍数，UI才会进行更新
                            if (readSum / UploadConfig.ShowSize > pos || readSum == stream.Length)
                            {
                                pos = readSum / UploadConfig.ShowSize;
                                vo.UploadedSize = readSum;
                            }
                        }
                        var fragmentInfo = new FragmentInfo { Index = index, Size = readAmount };

                        if (vo.State != UploadFileState.Uploading)
                        {
                            return;
                        }

                        byte[] tmpBuf = new byte[readAmount];
                        Array.Copy(buf, tmpBuf, readAmount);

                        // 开始分片的远程上传
                        UploadManager.GetInstance().RunUpload(tmpBuf, fragmentInfo, async(isOk) => {
                            if (!isOk)
                            {
                                // 上传失败
                                vo.State = UploadFileState.Fail;
                                return;
                            }

                            task.FragmentNum--;
                            task.FileOffset += fragmentInfo.Size;
                            if (task.FragmentNum == 0)
                            {
                                var res = await uploadService.Complete("token");
                                if (res.StatusCode == ServiceHelper.RequestOk)
                                {
                                    vo.State = UploadFileState.Success;
                                } else {
                                    vo.State = UploadFileState.Fail;
                                }

                                lock (mtx)
                                {
                                    // UploadTaskSize 和 SecondUploaded 为线程的共享内存变量
                                    UploadTaskSize--;
                                    if (UploadTaskSize == 0)
                                    {
                                        _timer.Stop();
                                    }
                                }
                            }
                            lock(mtx)
                            {
                                SecondUploaded += readAmount;
                            }
                        });
                    }
                }
            });
        }
    }
}
