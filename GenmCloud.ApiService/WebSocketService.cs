using GenmCloud.Core.Event;
using GenmCloud.Shared.Common;
using GenmCloud.Shared.Common.Aop;
using GenmCloud.Shared.Common.Session;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocket4Net;

namespace GenmCloud.ApiService
{
    public class WebSocketRecvEvent : PubSubEvent<string> { }
    public class WebSocketSendEvent : PubSubEvent<string> { }
    public class WebSocketConnectedEvent : PubSubEvent { }
    public class WebSocketClosedEvent : PubSubEvent { }
    public class WebSocketErrorEvent : PubSubEvent { }

    public class WebSocketService
    {
        private string serverUrl = "ws://localhost:1026/api/v1/ws";
        private readonly IEventAggregator eventAggregator;
        private readonly WebSocket conn;
        private bool isConnected = false;
        private object mutex = new object();

        public WebSocketService(IEventAggregator eventAggregator)
        {
            // TODO 需要增加断线重连

            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<WebSocketSendEvent>().Subscribe(Send);
            conn = new WebSocket(serverUrl + "?token=" + SessionService.Token);
            conn.Opened += OnOpened;
            conn.MessageReceived += OnReceived;
            conn.Error += OnError;
            conn.Closed += OnClosed;
            conn.Open();
        }

        [GlobalLoger]
        private void OnClosed(object sender, EventArgs e)
        {
            lock (mutex)
            {
                isConnected = false;
                this.eventAggregator.GetEvent<WebSocketClosedEvent>().Publish();
            }
        }

        [GlobalLoger]
        private void OnError(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            lock(mutex)
            {
                isConnected = false;
                this.eventAggregator.GetEvent<WebSocketErrorEvent>().Publish();
            }
        }

        [GlobalLoger]
        private void OnReceived(object sender, MessageReceivedEventArgs e)
        {
            this.eventAggregator.GetEvent<WebSocketRecvEvent>().Publish(e.Message);
        }

        [GlobalLoger]
        private void OnOpened(object sender, EventArgs e)
        {
            lock(mutex)
            {
                isConnected = true;
                this.eventAggregator.GetEvent<WebSocketConnectedEvent>().Publish();
            }
        }

        [GlobalLoger]
        public void Send(string message)
        {
            lock(mutex)
            {
                // 断线重连的触发点之一
                if (!isConnected)
                {
                    TryRun();
                }
                else
                {
                    conn.Send(message);
                }
            }
        }

        private void TryRun()
        {
            lock(mutex)
            {
                if (!isConnected)
                {
                    conn.Open();
                }
            }
        }


        public void SetToken(string token)
        {
            serverUrl += "?token=" + token;
        }
        
    }
}
