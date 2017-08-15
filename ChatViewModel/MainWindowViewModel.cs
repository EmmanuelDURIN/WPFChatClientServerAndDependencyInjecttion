using ChatBusinessObjects;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatViewModel
{
  public class MainWindowViewModel : BindableBase
  {
    private ObservableCollection<ChatMessage> messages;
    private User user;
    private ChatMessage messageToSend;

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
