using ChatBusinessLogic;
using ChatBusinessObjects;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBusinessLogic
{
  // Même interface que IChatCommunication sauf que émission d'événements pour les clients graphiques 
  public interface INotifyingClientChatCommunication : IChatCommunication
  {
    Action<ChatMessage> MessageReceived { get; set; }
    event Action<StateChange> StateChanged;
    event Action<String> UserConnected;
    event Action<String> UserDisconnected;
  }
}
