using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenmCloud.Core.Manager
{
    interface IAvatarManager
    {
        void Store(uint id, string avatar);

        string Get(uint id);
    }
}
