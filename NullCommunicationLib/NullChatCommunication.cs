using ChatBusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatBusinessLogic
{
  public class NullChatCommunication : IClientChatCommunication
  {
    public event Action<ChatMessage> MessageReceived;
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
    public async Task SendMessage(ChatMessage message)
    {
      Task task = Task.Delay(2000);
      await task;
    }
  }
}
