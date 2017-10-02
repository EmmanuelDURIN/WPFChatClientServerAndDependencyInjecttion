using ChatBusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatBusinessLogic
{
  [Export(typeof(IChatCommunication))]
  public class NullChatCommunication : IChatCommunication
  {
    public event Action<ChatMessage> MessageReceived;
    public async Task Connect(string userName, string password, CancellationToken token)
    {
      Task task = Task.Delay(2000, token);
      await task;
    }
    public async Task Disconnect(CancellationToken token)
    {
      Task task = Task.Delay(2000, token);
      await task;
    }
    public async Task SendMessage(ChatMessage message)
    {
      Task task = Task.Delay(2000);
      await task;
    }
  }
}