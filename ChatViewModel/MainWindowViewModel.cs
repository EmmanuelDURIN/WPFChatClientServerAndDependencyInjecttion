using ChatBusinessLogic;
using ChatBusinessObjects;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Commands;

namespace ChatViewModel
{
  public class MainWindowViewModel : BindableBase
  {
    private ObservableCollection<ChatMessage> messages = new ObservableCollection<ChatMessage>();
    private User user = new User();
    private ChatMessage messageToSend = new ChatMessage();
    private IChatCommunication chatCommunication = new NullChatCommunication();
    private bool isSending;
    private bool isConnecting;

    public RelayCommand ConnectCmd { get; set; }
    public RelayCommand DisconnectCmd { get; set; }
    public RelayCommand SendMessageCmd { get; set; }

    public bool IsConnecting
    {
      get { return isConnecting; }
      set
      {
        bool hasChanged = SetProperty(ref isConnecting, value);
        if (hasChanged) { 
          OnPropertyChanged(nameof(AnyCommandRunning));
          ConnectCmd.FireExecuteChanged();
          DisconnectCmd.FireExecuteChanged();
          SendMessageCmd.FireExecuteChanged();
        }
      }
    }

    public bool IsSending
    {
      get { return isSending; }
      set
      {
        bool hasChanged = SetProperty(ref isSending, value);
        if (hasChanged)
        {
          OnPropertyChanged(nameof(AnyCommandRunning));
          ConnectCmd.FireExecuteChanged();
          DisconnectCmd.FireExecuteChanged();
          SendMessageCmd.FireExecuteChanged();
        }
      }
    }
    private bool AnyCommandRunning
    {
      get {
        return isSending || isConnecting;
      }
    }
    public ObservableCollection<ChatMessage> Messages
    {
      get { return messages; }
      set
      {
        SetProperty(ref messages, value);
      }
    }

    public User User
    {
      get { return user; }
      set
      {
        SetProperty(ref user, value);
      }
    }

    public ChatMessage MessageToSend
    {
      get { return messageToSend; }
      set
      {
        SetProperty(ref messageToSend, value);
      }
    }
    public MainWindowViewModel()
    {
      LoadDummyData();

      ConnectCmd = new RelayCommand(execute: Connect, canExecute: o => !AnyCommandRunning); 
      DisconnectCmd = new RelayCommand(execute: Disconnect, canExecute: o => !AnyCommandRunning);
      SendMessageCmd = new RelayCommand(execute: SendMessage, canExecute: o => !AnyCommandRunning );
    }
    private void SendMessage(object obj)
    {
      IsSending = true;
      MessageToSend.EmissionDate = DateTime.Now;
      MessageToSend.Speaker = User.Name;
      chatCommunication.SendMessage(MessageToSend);
      MessageToSend.Content = "";
      IsSending = false;
    }
    private void Connect(object obj)
    {
      IsConnecting = true;
      chatCommunication.Connect(User.Name, User.Password);
      IsConnecting = false;
    }
    private void Disconnect(object obj)
    {
      IsConnecting = true;
      chatCommunication.Disconnect();
      IsConnecting = false;
    }	
    private void LoadDummyData()
    {
      var newMessages = Enumerable.Range(1, 10)
        .Select(i => new ChatMessage
        {
          Speaker = "Emmanuel",
          Content = "Hello" + i,
          EmissionDate = DateTime.Now.AddDays(-10).AddMinutes(i)
        }
        );
      foreach (var message in newMessages)
      {
        messages.Add(message);
      }
    }
  }
}
