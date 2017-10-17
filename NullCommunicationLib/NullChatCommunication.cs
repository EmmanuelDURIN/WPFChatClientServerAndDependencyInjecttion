using ChatBusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace ChatBusinessLogic
{
  public class NullChatCommunication : IClientChatCommunication
  {
    public Action<ChatMessage> MessageReceived { get; set; }

    public event Action<StateChange> StateChanged;

    public async Task Connect(string userName, string password)
    {
      Task task = Task.Delay(2000);
      await task;
    }
    public async Task Disconnect()
    {
      Task task = Task.Delay(2000);
      await task;

    }
    public Task<List<User>> GetConnectedUsers()
    {
      return Task.FromResult<List<User>>(
      Enumerable.Range(1,3)
        .Select( i => 
          new User { Name = "User"+i , Password="Password"+i }
      )
      .ToList());
    }

    public async Task SendMessage(ChatMessage message)
    {
      Task task = Task.Delay(2000);
      await task;
    }
  }
}
