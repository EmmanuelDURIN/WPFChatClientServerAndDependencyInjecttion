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
      Thread.Sleep(2000);
    }
    public void Disconnect()
    {
      Thread.Sleep(2000);
    }
    public void SendMessage(ChatMessage message)
    {
      Thread.Sleep(2000);
    }
  }
}
