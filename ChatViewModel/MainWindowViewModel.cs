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
    private ObservableCollection<ChatMessage> messages;
    private User user;
    private ChatMessage messageToSend;
    private IChatCommunication chatCommunication = new NullChatCommunication();
    private bool isSending;
    private bool isConnecting;

    public RelayCommand ConnectCmd { get; set; }
    public RelayCommand DisconnectCmd { get; set; }
    public RelayCommand SendMessageCmd { get; set; }

    public MainWindowViewModel()
    {
      ConnectCmd = new RelayCommand(execute: Connect, canExecute: o => !AnyCommandRunning && !String.IsNullOrWhiteSpace(User.Name) && !String.IsNullOrWhiteSpace(User.Password));
      DisconnectCmd = new RelayCommand(execute: Disconnect, canExecute: o => !AnyCommandRunning);
      SendMessageCmd = new RelayCommand(execute: SendMessage, canExecute: o => !AnyCommandRunning && !String.IsNullOrWhiteSpace(MessageToSend.Content) );
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
    //private bool? oldAnyCommandRunning;
    private bool AnyCommandRunning
    {
      get {
        //bool newAnyCommandRunning = isSending || isConnecting;
        //if( !oldAnyCommandRunning.HasValue || newAnyCommandRunning != oldAnyCommandRunning.Value)
        //{
        //  // le faire avnt les Fire pour éviter récursion infinie
        //  oldAnyCommandRunning = newAnyCommandRunning;
        //  SendMessageCmd.FireExecuteChanged();
        //  ConnectCmd.FireExecuteChanged();
        //  DisconnectCmd.FireExecuteChanged();
        //}
        //// le faire quoiqu'il en soit
        //oldAnyCommandRunning = newAnyCommandRunning;
        //return newAnyCommandRunning;
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
  }
}
