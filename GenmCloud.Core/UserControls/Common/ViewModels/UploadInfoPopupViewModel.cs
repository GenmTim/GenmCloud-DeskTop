using GenmCloud.ApiService.Service;
using GenmCloud.Core.Data.VO;
using GenmCloud.Core.Event;
using GenmCloud.Shared.Common;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace GenmCloud.Core.UserControls.Common.ViewModels
{

    public class UploadInfoPopupViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private IFileService fileService;

        public UploadInfoPopupViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.fileService = NetCoreProvider.Resolve<IFileService>();
            eventAggregator.GetEvent<UploadFileEvent>().Subscribe(UploadFile);
            ClosePopupCmd = new DelegateCommand(() => {
                IsShowPopup = false;
            });
        }

        #region Property
        private int uploadFileItemNumber;
        public int UploadFileItemNumber
        {
            get => uploadFileItemNumber;
            set
            {
                uploadFileItemNumber = value;
                RaisePropertyChanged();
            }
        }


        private long uploadFileSumSize;
        public long UploadFileSumSize
        {
            get => uploadFileSumSize;
            set
            {
                uploadFileSumSize = value;
                RaisePropertyChanged();
            }
        }

        private long uploadedFileSumSize;
        public long UploadedFileSumSize
        {
            get => uploadedFileSumSize;
            set
            {
                uploadedFileSumSize = value;
                RaisePropertyChanged();
            }
        }

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




        private ObservableCollection<UploadFileItemVO> uploadFileItemList = new ObservableCollection<UploadFileItemVO>();
        public ObservableCollection<UploadFileItemVO> UploadFileItemList
        {
            get => uploadFileItemList;
            set
            {
                uploadFileItemList = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand ClosePopupCmd { get; private set; }

        #endregion

        private void UploadFile(UploadFileTask fileTask)
        {
            // 构建文件上传项视图
            UploadFileItemVO uploadFileItemVO = new UploadFileItemVO
            {
                State = UploadFileState.上传中,
                Size = fileTask.FileSize,
                FullName = fileTask.FilePath,
                Name = fileTask.FileName,
            };

            UploadFileItemList.Add(uploadFileItemVO);
            UploadFileSumSize += uploadFileItemVO.Size;
            UploadFileItemNumber += 1;

            Task.Run(async () =>
            {
                var result = await fileService.Upload(fileTask.FilePath);
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    IsShowPopup = true;
                }));
                //FileStream fs = File.OpenRead(fileItem.FullName);
                //long readedSize = 0;
                //byte[] buffer = new byte[1024 * 1024];
                //while (readedSize < fileSize)
                //{
                //    int readSize = fs.Read(buffer, 0, buffer.Length);
                //    readedSize += readSize;
                //    // 发送到服务端

                //    // 计算上传比例，传递到UI上
                //    long nowRate = readedSize * 100 / fileSize;
                //    Application.Current.Dispatcher.Invoke(new Action(() =>
                //    {
                //        UploadedFileSumSize += readSize;
                //        foreach (var item in UploadFileItemList)
                //        {
                //            if (item.Id == fileItem.Id)
                //            {
                //                item.Rate = nowRate;
                //            }
                //        }
                //    }));
                //}
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    UploadFileItemNumber -= 1;
                    uploadFileItemVO.State = UploadFileState.上传完成;
                    this.eventAggregator.GetEvent<UploadedFileEvent>().Publish(fileTask);
                }));
                //fs.Close();
            });
        }
    }
}
