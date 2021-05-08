using Genm.WPF.Data.Event;
using GenmCloud.Shared.HttpContact;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenmCloud.Core.Tools.Helper
{
    public static class ToastHelper
    {
        public static void ShowServerError(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<ToastShowEvent>().Publish("服务器出了点小问题d(ŐдŐ๑)");
        }

        public static void ShowCommonError(IEventAggregator eventAggregator, BaseResponse response)
        {
            eventAggregator.GetEvent<ToastShowEvent>().Publish(response.Message + "d(ŐдŐ๑)");
        }

        public static void JudgeErrorAndShow(IEventAggregator eventAggregator, BaseResponse response)
        {
            if (response == null)
            {
                ToastHelper.ShowServerError(eventAggregator);
            }
            else if (response.StatusCode != 0)
            {
                ToastHelper.ShowCommonError(eventAggregator, response);
            }
        }
    }
}
