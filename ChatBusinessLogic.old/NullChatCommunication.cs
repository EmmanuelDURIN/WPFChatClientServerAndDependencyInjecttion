using ChatBusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatBusinessLogic
{
  public class NullChatCommunication : IChatCommunication
  {
    public event Action<ChatMessage> MessageReceived;
    public void Connect(string userName, string password)
    {
      Task task = Task.Delay(2000);
      task.Wait();
    }
    public void Disconnect()
    {
      Task task = Task.Delay(2000);
      task.Wait();
    }
    public void SendMessage(ChatMessage message)
    {
      Task task = Task.Delay(2000);
      task.Wait();
    }
  }
}
