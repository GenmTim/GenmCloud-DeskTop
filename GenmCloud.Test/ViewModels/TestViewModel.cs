using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace GenmCloud.Test.ViewModels
{
    public class FileItemVO
    {
        public string FileName { get; set; }
        public string OwnerName { get; set; }
        public string CreatedAt { get; set; }
    }

    public class TestViewModel : BindableBase
    {
        private ObservableCollection<FileItemVO> fileItemVOList = new ObservableCollection<FileItemVO>();
        public ObservableCollection<FileItemVO> FileItemVOList { get => fileItemVOList; set => fileItemVOList = value; }

        public TestViewModel()
        {

            FileItemVOList.Add(new FileItemVO { FileName = "任务", OwnerName = "蔡承龙", CreatedAt = "3月22日 21:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "行业分类——2018国家统计局数据", OwnerName = "余浩臻", CreatedAt = "3月22日 23:15" });
            FileItemVOList.Add(new FileItemVO { FileName = "牛人认证", OwnerName = "蔡承龙", CreatedAt = "3月22日 23:12" });
            FileItemVOList.Add(new FileItemVO { FileName = "企业认证关键字段", OwnerName = "蔡承龙", CreatedAt = "3月22日 13:32" });
            FileItemVOList.Add(new FileItemVO { FileName = "企业认证", OwnerName = "余浩臻", CreatedAt = "3月22日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
        }

    }
}
