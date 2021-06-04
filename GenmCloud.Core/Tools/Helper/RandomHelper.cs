using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenmCloud.Core.Tools.Helper
{
    public static class RandomHelper
    {
        public static uint RandUInt()
        {
            Random ran = new Random();
            return (uint)ran.Next(100, 999);
        }
    }
}
