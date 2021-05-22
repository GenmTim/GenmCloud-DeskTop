using Genm.WPF.Data.Event;
using GenmCloud.Shared.HttpContact;
using Prism.Events;

namespace GenmCloud.Core.Tools.Helper
{
    public static class ToastHelper
    {
        public static void ShowServerError(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<ToastShowEvent>().Publish("服务器出了点小问题d(ŐдŐ๑)");
        }

        public static void ShowCommonError(IEventAggregator eventAggregator, string message)
        {
            eventAggregator.GetEvent<ToastShowEvent>().Publish(message + "d(ŐдŐ๑)");
        }

        public static void JudgeErrorAndShow(IEventAggregator eventAggregator, BaseResponse response)
        {
            if (response == null || response.StatusCode == 0)
            {
                ToastHelper.ShowServerError(eventAggregator);
            }
            else if (response.StatusCode != ServiceHelper.RequestOk)
            {
                ToastHelper.ShowCommonError(eventAggregator, response.Message);
            }
        }

        public static void JudgeErrorAndShow<T>(IEventAggregator eventAggregator, BaseResponse<T> response)
        {
            if (response == null || response.StatusCode == 0)
            {
                ToastHelper.ShowServerError(eventAggregator);
            }
            else if (response.StatusCode != ServiceHelper.RequestOk)
            {
                ToastHelper.ShowCommonError(eventAggregator, response.Message);
            }
        }
    }
}
