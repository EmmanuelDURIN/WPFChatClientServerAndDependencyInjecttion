using ChatBusinessLogic;
using ChatBusinessObjects;
using Model;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ViewModel.Commands;

namespace ChatViewModel
{
    public class MainWindowViewModel : ReactiveObject//: BindableBase
    {
        private ObservableCollection<ChatMessage> messages = new ObservableCollection<ChatMessage>();
        private User user = new User();
        private ChatMessage messageToSend = new ChatMessage();
        private IChatCommunication chatCommunication = new NullChatCommunication();
        private bool isSending;
        private bool isConnecting;
        private bool isConnected;
        private CancellationTokenSource connectionCts;

        public RelayCommand ConnectCmd { get; set; }
        public RelayCommand DisconnectCmd { get; set; }
        public RelayCommand SendMessageCmd { get; set; }
        public RelayCommand CancelConnectCmd { get; set; }

        public void PasswordChanged(string password)
        {
            User.Password = password;
            ConnectCmd.FireExecuteChanged();
        }

        public bool IsConnecting
        {
            get { return isConnecting; }
            set
            {
                isConnecting = value;
                //bool hasChanged = SetProperty(ref isConnecting, value);
                //if (hasChanged)
                //{
                //  OnPropertyChanged(nameof(AnyCommandRunning));
                //  ConnectCmd.FireExecuteChanged();
                //  DisconnectCmd.FireExecuteChanged();
                //  SendMessageCmd.FireExecuteChanged();
                //  CancelConnectCmd.FireExecuteChanged();
                //}
            }
        }

        public bool IsSending
        {
            get { return isSending; }
            set
            {
                isSending = value;
                //bool hasChanged = SetProperty(ref isSending, value);
                //if (hasChanged)
                //{
                //  OnPropertyChanged(nameof(AnyCommandRunning));
                //  ConnectCmd.FireExecuteChanged();
                //  DisconnectCmd.FireExecuteChanged();
                //  SendMessageCmd.FireExecuteChanged();
                //}
            }
        }
        private bool anyCommandRunning;
        public bool AnyCommandRunning
        {
            set
            {
                anyCommandRunning = value;
            }
            get
            {
                return anyCommandRunning;
            }
        }
        public ObservableCollection<ChatMessage> Messages
        {
            get { return messages; }
            set
            {
                messages = value;
            }
        }

        public User User
        {
            get { return user; }
            set
            {
                user = value;
            }
        }

        public ChatMessage MessageToSend
        {
            get { return messageToSend; }
            set
            {
                messageToSend = value;
            }
        }
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                //bool hasChanged = SetProperty(ref isConnected, value);
                //if (hasChanged)
                //{
                //  ConnectCmd.FireExecuteChanged();
                //  DisconnectCmd.FireExecuteChanged();
                //  SendMessageCmd.FireExecuteChanged();
                //  OnPropertyChanged(nameof(IsDisconnected));
                //}
            }
        }
        public bool IsDisconnected
        {
            get { return !isConnected; }
            //set { isConnected = !value; }
        }
        public MainWindowViewModel()
        {
            LoadDummyData();

            ConnectCmd = new RelayCommand(execute: Connect, canExecute: o => !AnyCommandRunning && !IsConnected && !String.IsNullOrWhiteSpace(User.Name) && !String.IsNullOrWhiteSpace(User.Password));
            DisconnectCmd = new RelayCommand(execute: Disconnect, canExecute: o => !AnyCommandRunning && IsConnected);
            SendMessageCmd = new RelayCommand(execute: SendMessage, canExecute: o => !AnyCommandRunning && !String.IsNullOrWhiteSpace(MessageToSend.Content) && IsConnected);
            CancelConnectCmd = new RelayCommand(execute: CancelConnect, canExecute: o => IsConnecting);
            
            // Ce qu'on écoute et ce qu'on produit en sortie de l'écoute
            IObservable<bool> observableAnyCommandRunning = this.WhenAnyValue
                (
                    x => x.IsConnecting, 
                    x => x.IsSending, 
                    (isConnecting, isSending) => isSending || isConnecting
                );
            // Dans quelle propriété on écrit 
            ObservableAsPropertyHelper<bool> helper = observableAnyCommandRunning.ToProperty(this, x => x.AnyCommandRunning);

            //this
            //    .WhenAny(x => x.IsConnected, observedChange => observedChange.IsConnected)
            //    // Raise event that IsDisconnected has changed
            //    .Subscribe(x => x)
            ////    .ToProperty(this, x => x.IsDisconnected);

            User.PropertyChanged += (o, args) =>
              {
                  if (args.PropertyName == nameof(User.Name) || args.PropertyName == nameof(User.Password))
                      ConnectCmd.FireExecuteChanged();
              };
            MessageToSend.PropertyChanged += (o, args) =>
            {
                if (args.PropertyName == nameof(ChatMessage.Content))
                    SendMessageCmd.FireExecuteChanged();
            };
        }
        private async void SendMessage(object obj)
        {
            IsSending = true;
            MessageToSend.EmissionDate = DateTime.Now;
            MessageToSend.Speaker = User.Name;
            await chatCommunication.SendMessage(MessageToSend);
            MessageToSend.Content = "";
            IsSending = false;
        }
        private async void Connect(object obj)
        {
            IsConnecting = true;
            connectionCts = new CancellationTokenSource();
            try
            {
                await chatCommunication.Connect(User.Name, User.Password, connectionCts.Token);
                IsConnected = true;
            }
            catch (TaskCanceledException)
            {
                System.Diagnostics.Debug.WriteLine("Cancellation");
            }
            finally
            {
                IsConnecting = false;
            }
        }
        private async void Disconnect(object obj)
        {
            IsConnecting = true;
            connectionCts = new CancellationTokenSource();
            try
            {
                await chatCommunication.Disconnect(connectionCts.Token);
                IsConnected = false;
            }
            catch (TaskCanceledException)
            {
                System.Diagnostics.Debug.WriteLine("Cancellation");
            }
            finally
            {
                IsConnecting = false;
            }
        }
        private void CancelConnect(object obj)
        {
            connectionCts.Cancel(throwOnFirstException: false);
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
