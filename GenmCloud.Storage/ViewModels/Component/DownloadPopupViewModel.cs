using GenmCloud.ApiService.Service;
using GenmCloud.ApiService.Service.Impl;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Shared.Common;
using GenmCloud.Storage.Data.Event;
using GenmCloud.Storage.Data.Task;
using GenmCloud.Storage.Data.VO;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GenmCloud.Storage.ViewModels.Component
{
    class DownloadPopupViewModel : BindableBase
    {
        private long downloadSpeed;
        public long DownloadSpeed
        {
            get => downloadSpeed;
            set
            {
                downloadSpeed = value;
                RaisePropertyChanged();
            }
        }

        private long SecondDownload;

        private DispatcherTimer _timer = new DispatcherTimer();


        #region Property
        // 表示所有任务上传完成
        private bool isDownloadCompleted;
        public bool IsDownloadCompleted
        {
            get => isDownloadCompleted;
            set
            {
                isDownloadCompleted = value;
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
                IsDownloadCompleted = (processRate == 100);
                ProcessBorderLength = 460 * processRate / 100;
                RaisePropertyChanged();
            }
        }

        // 总上传任务数
        private int downloadFileItemNumber;
        public int DownloadFileItemNumber
        {
            get => downloadFileItemNumber;
            set
            {
                downloadFileItemNumber = value;
                if (downloadFileItemNumber == 0)
                {
                    if (_timer != null)
                    {
                        _timer.Stop();
                    }
                }
                else
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
        private long downloadFileSumSize;
        public long DownloadFileSumSize
        {
            get => downloadFileSumSize;
            set
            {
                downloadFileSumSize = value;
                UpdateRate();
                RaisePropertyChanged();
            }
        }

        // 已上传任务的数据总量
        private long downloadedFileSumSize;
        public long DownloadedFileSumSize
        {
            get => downloadedFileSumSize;
            set
            {
                downloadedFileSumSize = value;
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

        private ObservableCollection<DownloadFileItemVO> downloadFileList = new ObservableCollection<DownloadFileItemVO>();
        public ObservableCollection<DownloadFileItemVO> DownloadFileList
        {
            get => downloadFileList;
            set
            {
                downloadFileList = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand ClosePopupCmd { get; private set; }
        public DelegateCommand<object> PauseCmd { get; private set; }
        public DelegateCommand<object> StartCmd { get; private set; }
        public DelegateCommand<object> SingleTaskCmd { get; private set; }
        #endregion


        private readonly IEventAggregator eventAggregator;
        private readonly DownloadService downloadService;



        public class DownloadHeightEvent : PubSubEvent { }

        public DownloadPopupViewModel()
        {
            this.eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();
            this.downloadService = NetCoreProvider.Resolve<DownloadService>();

            ClosePopupCmd = new DelegateCommand(() =>
            {
                IsShowPopup = false;
            });

            StartCmd = new DelegateCommand<object>(StartDownload);

            PauseCmd = new DelegateCommand<object>(PauseDownload);

            SingleTaskCmd = new DelegateCommand<object>((obj) =>
            {
                if (obj is DownloadFileItemVO vo)
                {
                    if (vo.State == DownloadFileState.Downloading)
                    {
                        PauseDownload(vo);
                    }
                    else
                    {
                        StartDownload(vo);
                    }
                }
            });

            _timer.Interval = new TimeSpan(0, 0, 1);   //间隔1秒
            _timer.Tick += new EventHandler((s, e) =>
            {
                DownloadSpeed = SecondDownload;
                SecondDownload = 0;
            });
            eventAggregator.GetEvent<DownloadFileEvent>().Subscribe(NewDownloadTask);
        }

        private void UpdateRate()
        {
            if (downloadFileSumSize != 0)
            {
                ProcessRate = downloadedFileSumSize * 100 / downloadFileSumSize;
            }
        }

        private void StartDownload(object obj)
        {
            if (obj == null)
            {
                foreach (var vo in DownloadFileList)
                {
                    if (vo.State == DownloadFileState.Pause)
                    {
                        StartDownloadTask(vo);
                    }
                }
            }
            else if (obj is DownloadFileItemVO vo)
            {
                if (vo.State == DownloadFileState.Pause)
                {
                    StartDownloadTask(vo);
                }
            }
        }

        private void PauseDownload(object obj)
        {
            if (obj == null)
            {
                foreach (var vo in DownloadFileList)
                {
                    if (vo.State == DownloadFileState.Downloading)
                    {
                        vo.State = DownloadFileState.Pause;
                    }
                }
            }
            else if (obj is DownloadFileItemVO vo)
            {
                if (vo.State == DownloadFileState.Downloading)
                {
                    vo.State = DownloadFileState.Pause;
                }
            }
        }

        private void NewDownloadTask(DownloadFileTask task)
        {
            // 添加UI项到列表中，并维护上传任务
            var vo = new DownloadFileItemVO { FullName = task.FileName, Name = task.FileName, Size = task.FileSize, Task = task };
            vo.Task.TargetStream ??= File.OpenWrite(vo.Task.TargetAddr + @"\\" + vo.Task.FileName);

            DownloadFileList.Add(vo);
            DownloadFileSumSize += vo.Size;

            IsShowPopup = true;

            eventAggregator.GetEvent<DownloadHeightEvent>().Publish();

            // 增量当前任务数
            StartDownloadTask(vo);
        }

        private async void StartDownloadTask(DownloadFileItemVO vo)
        {
            IncreaseTaskNum();
            vo.State = DownloadFileState.Downloading;
            await Task.Run(() => {
                lock(vo)
                {
                    if (vo.State != DownloadFileState.Downloading) return;
                    var task = vo.Task;
                    var destFile = task.TargetStream;
                    var success = false;
                    downloadService.DownloadFile(task.FileID, destFile.Position, vo.Task.FileSize, (stream) =>
                    {
                        var bufSize = 4 * 1024;
                        var buf = new byte[bufSize];
                        while (vo.State == DownloadFileState.Downloading)
                        {
                            int readn = stream.Read(buf, 0, bufSize);
                            if (readn == 0)
                            {
                                success = true;
                                return;
                            }
                            destFile.Write(buf, 0, readn);
                            vo.DownloadedSize += readn;
                            SecondDownload += readn;
                            DownloadedFileSumSize += readn;
                        }
                    });
                    if (success || destFile.Position == vo.Task.FileSize)
                    {
                        DownloadedSingleFile(vo);
                    }
                }
            });
            DecreaseTaskNum();

        }

        private void DownloadedSingleFile(DownloadFileItemVO vo)
        {
            vo.State = DownloadFileState.Success;
            vo.Task.TargetStream.Close();
            vo.Task.TargetStream = null;
        }

        private void DecreaseTaskNum()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                DownloadFileItemNumber--;
                if (DownloadFileItemNumber == 0)
                {
                    IsDownloadCompleted = true;
                    _timer.Stop();
                }
            });

        }

        private void IncreaseTaskNum()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                DownloadFileItemNumber++;
                _timer.Start();
            });
        }
    }
}
