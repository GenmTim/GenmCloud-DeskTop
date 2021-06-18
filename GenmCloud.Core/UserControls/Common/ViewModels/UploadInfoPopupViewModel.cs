using GenmCloud.ApiService.Service;
using GenmCloud.ApiService.Service.Impl;
using GenmCloud.Core.Data.VO;
using GenmCloud.Core.Event;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Shared.Common;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GenmCloud.Core.UserControls.Common.ViewModels
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

    public class UploadInfoPopupViewModel : BindableBase
    {
        private long uploadSpeed;
        public long UploadSpeed
        {
            get => uploadSpeed;
            set
            {
                uploadSpeed = value;
                RaisePropertyChanged();
            }
        }

        private long SecondUploaded;

        private DispatcherTimer _timer = new DispatcherTimer();


        #region Property
        // 表示所有任务上传完成
        private bool isUploadCompleted;
        public bool IsUploadCompleted
        {
            get => isUploadCompleted;
            set
            {
                isUploadCompleted = value;
                RaisePropertyChanged();
            }
        }

        // 进度条长度
        private long processBorderLength;
        public long ProcessBorderLength
        {
            get => processBorderLength;
            set
            {
                processBorderLength = value;
                RaisePropertyChanged();
            }
        }

        // 总进度比
        private long processRate;
        public long ProcessRate
        {
            get => processRate;
            set
            {
                processRate = value;
                IsUploadCompleted = (processRate == 100);
                ProcessBorderLength = 460 * processRate / 100;
                RaisePropertyChanged();
            }
        }

        // 总上传任务数
        private int uploadFileItemNumber;
        public int UploadFileItemNumber
        {
            get => uploadFileItemNumber;
            set
            {
                uploadFileItemNumber = value;
                if (uploadFileItemNumber == 0)
                {
                    if (_timer != null)
                    {
                        _timer.Stop();
                    }
                } else
                {
                    if (_timer != null && _timer.IsEnabled)
                    {
                        _timer.Start();
                    }
                }
                RaisePropertyChanged();
            }
        }


        // 上传任务的数据总量
        private long uploadFileSumSize;
        public long UploadFileSumSize
        {
            get => uploadFileSumSize;
            set
            {
                uploadFileSumSize = value;
                UpdateRate();
                RaisePropertyChanged();
            }
        }

        // 已上传任务的数据总量
        private long uploadedFileSumSize;
        public long UploadedFileSumSize
        {
            get => uploadedFileSumSize;
            set
            {
                uploadedFileSumSize = value;
                UpdateRate();
                RaisePropertyChanged();
            }
        }

        // 上传视图是否伸展状态
        private bool isOpenPopup;
        public bool IsOpenPopup
        {
            get => isOpenPopup;
            set
            {
                isOpenPopup = value;
                RaisePropertyChanged();
            }
        }

        // 上传视图是否可视状态
        private bool isShowPopup;
        public bool IsShowPopup
        {
            get => isShowPopup;
            set
            {
                isShowPopup = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<UploadFileItemVO> uploadFileList = new ObservableCollection<UploadFileItemVO>();
        public ObservableCollection<UploadFileItemVO> UploadFileList
        {
            get => uploadFileList;
            set
            {
                uploadFileList = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand ClosePopupCmd { get; private set; }
        public DelegateCommand<object> PauseCmd { get; private set; }
        public DelegateCommand<object> StartCmd { get; private set; }
        public DelegateCommand<object> SingleTaskCmd { get; private set; }
        #endregion


        private readonly IEventAggregator eventAggregator;
        private readonly IUploadService uploadService;



        public class UploadHeightEvent : PubSubEvent { }

        public UploadInfoPopupViewModel()
        {
            this.eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();
            this.uploadService = NetCoreProvider.Resolve<IUploadService>();

            eventAggregator.GetEvent<UploadFileEvent>().Subscribe(NewUploadTask);

            ClosePopupCmd = new DelegateCommand(() =>
            {
                IsShowPopup = false;
            });

            StartCmd = new DelegateCommand<object>(StartUpload);

            PauseCmd = new DelegateCommand<object>(PauseUpload);

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
                UploadSpeed = SecondUploaded;
                SecondUploaded = 0;
            });
        }

        private void UpdateRate()
        {
            if (uploadFileSumSize != 0)
            {
                ProcessRate = uploadedFileSumSize * 100 / uploadFileSumSize;
            }
        }

        private void StartUpload(object obj)
        {
            if (obj == null)
            {
                foreach (var vo in UploadFileList)
                {
                    if (vo.State == UploadFileState.Pause)
                    {
                        StartUploadTask(vo);
                    }
                }
            }
            else if (obj is UploadFileItemVO vo)
            {
                if (vo.State == UploadFileState.Pause)
                {
                    StartUploadTask(vo);
                }
            }
        }

        private void PauseUpload(object obj)
        {
            if (obj == null)
            {
                foreach (var vo in UploadFileList)
                {
                    if (vo.State == UploadFileState.Uploading)
                    {
                        vo.State = UploadFileState.Pause;
                        DecreaseTaskNum();
                    }
                }
            }
            else if (obj is UploadFileItemVO vo)
            {
                if (vo.State == UploadFileState.Uploading)
                {
                    vo.State = UploadFileState.Pause;
                    DecreaseTaskNum();
                }
            }
        }

        private void NewUploadTask(UploadFileTask task)
        {
            task.Sem = new Semaphore(1, 1);
            // 添加UI项到列表中，并维护上传任务
            var vo = new UploadFileItemVO { FullName = task.FileName, Name = task.FileName, Size = task.FileSize, Task = task };

            UploadFileList.Add(vo);
            UploadFileSumSize += vo.Size;

            IsShowPopup = true;

            eventAggregator.GetEvent<UploadHeightEvent>().Publish();

            // 增量当前任务数
            StartUploadTask(vo);
        }

        private async void StartUploadTask(UploadFileItemVO vo)
        {
            vo.Task.Sem.WaitOne();
            IncreaseTaskNum();
            vo.State = UploadFileState.Uploading;

            await Task.Run(async () => {
                var res = await uploadService.Prepare(vo.Task.FileName, vo.Task.FileSize, vo.Task.FolderId);
                if (res.StatusCode == ServiceHelper.RequestOk)
                {
                    var info = res.Result;
                    vo.Task.Token = info.Token;
                } else
                {
                    return;
                }

                var task = vo.Task;
                task.FragmentNum = (vo.Size - vo.Task.FileOffset + UploadConfig.FragmentSize - 1) / UploadConfig.FragmentSize;
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

                        byte[] tmpBuf = new byte[readAmount];
                        Array.Copy(buf, tmpBuf, readAmount);

                        var fragmentInfo = new FragmentInfo { Index = index, Size = readAmount, Token=task.Token };

                        // 开始分片的远程上传
                        UploadManager.GetInstance().RunUpload(tmpBuf, fragmentInfo, (isOK) => {
                            FragmentUploaded(isOK, fragmentInfo, vo);
                        });

                        if (vo.State != UploadFileState.Uploading)
                        {
                            goto streamEnd;
                        }
                    }
                }
                streamEnd:
                vo.Task.Sem.Release();
            });
        }

        private async void UploadedSingleFile(UploadFileItemVO vo)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (vo.State == UploadFileState.Uploading)
                {
                    DecreaseTaskNum();
                }
            });

            var res = await uploadService.Complete(vo.Task.Token);
            if (res.StatusCode == ServiceHelper.RequestOk)
            {
                vo.State = UploadFileState.Success;
                eventAggregator.GetEvent<UpdateFileListEvent>().Publish(null);
            }
            else
            {
                vo.State = UploadFileState.Fail;
            }
        }

        private void FragmentUploaded(bool isOk, FragmentInfo fragmentInfo, UploadFileItemVO vo)
        {
            if (!isOk)
            {
                UploadFail(vo);
                return;
            }

            var task = vo.Task;

            // 全局变量更新
            Application.Current.Dispatcher.Invoke(() => {
                SecondUploaded += fragmentInfo.Size;
                UploadedFileSumSize += fragmentInfo.Size;

                task.FragmentNum -= 1;
                task.FileOffset += fragmentInfo.Size;

                // 当前任务的所有分片，已经上传完成
                if (task.FileOffset == task.FileSize)
                {
                    UploadedSingleFile(vo);
                }
            });
        }

        private void UploadFail(UploadFileItemVO vo)
        {
            // 上传失败
            vo.State = UploadFileState.Fail;
        }

        private void DecreaseTaskNum()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                UploadFileItemNumber--;
                if (UploadFileItemNumber == 0)
                {
                    IsUploadCompleted = true;
                    _timer.Stop();
                }
            });

        }

        private void IncreaseTaskNum()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                UploadFileItemNumber++;
                _timer.Start();
            });
        }
    }
}
