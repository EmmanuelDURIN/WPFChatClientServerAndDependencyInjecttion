using ChatBusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatBusinessObjects;
using Microsoft.AspNet.SignalR.Client;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace SignalRChatClient
{
  public class ChatClient : INotifyingClientChatCommunication
  {
    const string baseUrl = "http://localhost:12983/";
    private IHubProxy hubProxy;
    private Authenticator authenticator = new Authenticator (baseUrl);
    private HubConnection hubConnection;
    public Action<ChatMessage> MessageReceived { get; set; }
    public event Action<StateChange> StateChanged;
    public event Action<string> UserConnected;
    public event Action<string> UserDisconnected;

    public async Task Connect(string userName, string password)
    {
      // Obtention du CookieContainer avec les cookies d'authentification :
      CookieContainer cookieContainer = await authenticator.Login(userName: userName, password: password);
      hubConnection = new HubConnection(baseUrl);
      // propagation des cookies d’authentification par websoket
      hubConnection.CookieContainer = cookieContainer;
      hubProxy = hubConnection.CreateHubProxy("ChatHub");

      ServicePointManager.DefaultConnectionLimit = 10;
      hubProxy.On<ChatMessage>(nameof(IClientChatCommunication.BroadcastMessage), OnReceiveMessage);
      hubProxy.On<String>(nameof(IClientChatCommunication.UserConnected), OnUserConnected);
      hubProxy.On<String>(nameof(IClientChatCommunication.UserDisconnected), OnUserDisconnected);
      await hubConnection.Start();
      await hubProxy.Invoke(nameof(IChatCommunication.Connect), userName, password);
      hubConnection.StateChanged += HubConnectionStateChanged;
      System.Diagnostics.Debug.WriteLine($"HubConnection.State : {hubConnection.State}");
      HubConnectionStateChanged(new StateChange (ConnectionState.Disconnected, hubConnection.State) );
    }

    private void OnUserDisconnected(string userName)
    {
      UserDisconnected?.Invoke(userName);
    }

    private void OnUserConnected(string userName)
    {
      UserConnected?.Invoke(userName);
    }

    private void HubConnectionStateChanged(StateChange state)
    {
      if (StateChanged != null)
      {
        StateChanged(state);
      }
    }
    private void OnReceiveMessage(ChatMessage message)
    {
      MessageReceived?.Invoke(message);
      System.Diagnostics.Debug.WriteLine("msg received");
    }
    public async Task Disconnect()
    {
      await hubProxy.Invoke(nameof(IChatCommunication.Disconnect));
      await authenticator.Logoff();
      hubConnection.Stop();
      hubConnection = null;
      // No way !
      // hubConnection.CookieContainer = new CookieContainer();
      hubProxy = null;
    }
    public async Task SendMessage(ChatMessage message)
    {
      await hubProxy.Invoke(nameof(IChatCommunication.SendMessage), message);
    }
    public async Task<List<User>> GetConnectedUsers()
    {
      return await hubProxy.Invoke<List<User>>(nameof(IChatCommunication.GetConnectedUsers));
    }
  }
}
