using System;

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
