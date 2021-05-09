using GenmCloud.Shared.Common;
using GenmCloud.Shared.Common.Aop;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocket4Net;

namespace GenmCloud.ApiService
{
    public class WebSocketRecvEvent : PubSubEvent<string> { }
    public class WebSocketSendEvent : PubSubEvent<string> { }

    public class WebSocketService
    {
        private readonly string serverUrl = "ws://localhost:1026/api/v1/chat";
        private readonly IEventAggregator eventAggregator;
        private readonly WebSocket conn;

        public WebSocketService(IEventAggregator eventAggregator)
        {
            // TODO 需要增加断线重连

            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<WebSocketSendEvent>().Subscribe(Send);
            conn = new WebSocket(serverUrl);
            conn.Opened += OnOpened;
            conn.MessageReceived += OnReceived;
            conn.Error += OnError;
            conn.Closed += OnClosed;
            conn.Open();
        }

        [GlobalLoger]
        private void OnClosed(object sender, EventArgs e)
        {
        }

        [GlobalLoger]
        private void OnError(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
        }

        [GlobalLoger]
        private void OnReceived(object sender, MessageReceivedEventArgs e)
        {
            this.eventAggregator.GetEvent<WebSocketRecvEvent>().Publish(e.Message);
        }

        [GlobalLoger]
        private void OnOpened(object sender, EventArgs e)
        {
        }

        [GlobalLoger]
        public void Send(string message)
        {
            //TODO Send 判断连接状态，为断线重连的触发点之一
            conn.Send(message);
        }

        private void Run()
        {
            conn.OpenAsync();
        }

        
    }
}
