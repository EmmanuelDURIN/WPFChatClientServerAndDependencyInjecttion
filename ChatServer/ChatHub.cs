using ChatBusinessLogic;
using ChatBusinessObjects;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace ChatServer
{
  public class ChatHub : Hub<IChatCommunication>, IChatCommunication
  {
    public Task Connect(string userName, string password)
    {
      return Task.FromResult(0);
    }
    [Authorize]
    public Task Disconnect()
    {
      System.Diagnostics.Debug.WriteLine($"Disconnect Caller is {this.Context.User.Identity.Name}");
      return Task.FromResult(0);
    }
    [Authorize]
    public Task SendMessage(ChatMessage message)
    {
      System.Diagnostics.Debug.WriteLine($"SendMessage Caller is {this.Context.User.Identity.Name}");
      //Clients.All.SendMessage(message);
      // Pour appeler tous sauf l'appelant :
      Clients.Others.SendMessage(message);
      return Task.FromResult(0);
    }
  }
}