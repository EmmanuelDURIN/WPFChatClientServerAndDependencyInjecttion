using ChatBusinessLogic;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using System.ComponentModel.Composition;

namespace ConnectedPeopleLib
{
  public class ConnectedPeopleViewModel
  {
    private IClientChatCommunication chatCommunication;
    public ConnectedPeopleViewModel(IClientChatCommunication chatCommunication)
    {
      this.chatCommunication = chatCommunication;
    }
  }
}