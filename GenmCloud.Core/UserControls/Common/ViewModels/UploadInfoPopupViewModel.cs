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

        public class UploadHeightEvent : PubSubEvent { }

        public UploadInfoPopupViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.fileService = NetCoreProvider.Resolve<IFileService>();
            eventAggregator.GetEvent<UploadFileEvent>().Subscribe(UploadFile);
            ClosePopupCmd = new DelegateCommand(() =>
            {
                IsShowPopup = false;
            });
        }

        #region Property

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

        private async void UploadFile(UploadFileTask fileTask)
        {
            // 构建文件上传项视图
            UploadFileItemVO uploadFileItemVO = new UploadFileItemVO
            {
                Id = fileTask.Id,
                State = UploadFileState.上传中,
                Size = fileTask.FileSize,
                FullName = fileTask.FilePath,
                Name = fileTask.FileName,
            };

            UploadFileItemList.Add(uploadFileItemVO);
            UploadFileSumSize += uploadFileItemVO.Size;
            UploadFileItemNumber += 1;
            eventAggregator.GetEvent<UploadHeightEvent>().Publish();
            ProcessRate = UploadedFileSumSize * 100 / UploadFileSumSize;

            var result = await fileService.Upload(fileTask.FolderId, fileTask.FilePath);

            IsShowPopup = true;
            UploadedFileSumSize += fileTask.FileSize;
            ProcessRate = UploadedFileSumSize * 100 / UploadFileSumSize;
            foreach (var item in UploadFileItemList)
            {
                if (item.Id == fileTask.Id)
                {
                    item.Rate = 100;
                }
            }

            UploadFileItemNumber -= 1;
            uploadFileItemVO.State = UploadFileState.上传完成;
            this.eventAggregator.GetEvent<UploadedFileEvent>().Publish(fileTask);
            this.eventAggregator.GetEvent<UpdateFileListEvent>().Publish(null);
        }
    }
}
