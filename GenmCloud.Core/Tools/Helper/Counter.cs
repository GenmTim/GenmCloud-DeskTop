namespace GenmCloud.Core.Tools.Helper
{
    public static class Counter
    {
        private static uint UploadTaskID = 0;
        public static uint NextUploadTaskID()
        {
            return UploadTaskID++;
        }
    }
}
