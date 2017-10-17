using ChatBusinessLogic;
using ChatBusinessObjects;
using Microsoft.Practices.Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;

namespace ConnectedPeopleLib
{
  public class ConnectedPeopleViewModel : BindableBase
  {
    private ObservableCollection<User> connectedPeople;
    public ObservableCollection<User> ConnectedPeople
    {
      get { return connectedPeople; }
      set
      {
        SetProperty(ref connectedPeople, value);
      }
    }
    private IClientChatCommunication chatCommunication;
    public ConnectedPeopleViewModel(IClientChatCommunication chatCommunication)
    {
      this.chatCommunication = chatCommunication;
    }
    public void Init()
    {
      chatCommunication.StateChanged += this.ChatCommunicationStateChanged;
    }
    private async void ChatCommunicationStateChanged(Microsoft.AspNet.SignalR.Client.StateChange stateChange)
    {
      if (stateChange.NewState == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
      {
        try
        {
          var users = await chatCommunication.GetConnectedUsers();
          ConnectedPeople = new ObservableCollection<User>(users);
        }
        catch (Exception)
        {
          System.Diagnostics.Debug.WriteLine("Exception during getting connected people");
        }
      }
    }
    internal void Close()
    {
      chatCommunication.StateChanged -= this.ChatCommunicationStateChanged;
    }
  }
}