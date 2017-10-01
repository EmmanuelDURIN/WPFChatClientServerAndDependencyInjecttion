using ChatBusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatBusinessObjects;
using Microsoft.AspNet.SignalR.Client;
using System.Net;

namespace SignalRChatClient
{
  public class ChatClient : IClientChatCommunication
  {
    private IHubProxy hubProxy;
    public Action<ChatMessage> MessageReceived { get; set; }
    public async Task Connect(string userName, string password)
    {
      var hubConnection = new HubConnection("http://localhost:12983/");
      hubProxy = hubConnection.CreateHubProxy("ChatHub");
      ServicePointManager.DefaultConnectionLimit = 10;
      hubProxy.On<ChatMessage>("SendMessage", OnSendMessage);
      await hubConnection.Start();
      await hubProxy.Invoke(nameof(IChatCommunication.Connect), userName, password);
    }
    private void OnSendMessage(ChatMessage message)
    {
      if (MessageReceived != null)
        MessageReceived(message);
      System.Diagnostics.Debug.WriteLine("msg received");
    }
    public async Task Disconnect()
    {
      await hubProxy.Invoke(nameof(IChatCommunication.Disconnect));
      hubProxy = null;
    }
    public async Task SendMessage(ChatMessage message)
    {
      await hubProxy.Invoke(nameof(IChatCommunication.SendMessage), message);
    }
  }
}
