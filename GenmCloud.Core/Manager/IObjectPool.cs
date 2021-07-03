using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenmCloud.Core.Manager
{
    public interface ObjectManager
    {
        // 获取对象池中的对象
        T Get<T>();

        // 创建指定对象的指定数量
        void Create<T>(int num);

        // 销毁指定对象
        void Clear<T>();
    }
}
