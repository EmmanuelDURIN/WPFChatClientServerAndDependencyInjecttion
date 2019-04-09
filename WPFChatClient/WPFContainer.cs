
using ChatBusinessLogic;
using ChatViewModel;
using Unity;
using Unity.Injection;

namespace WPFChatClient
{
  // That class inherits of UnityContainer
  public class WPFContainer : UnityContainer
  {
    #region Singleton Management Region
    private WPFContainer()
    {
      RegisterTypes();
    }
    private static WPFContainer instance;
    public static WPFContainer Instance
    {
      get
      {
        if (instance == null)
        {
          instance = new WPFContainer();
        }
        return instance;
      }
    }
    private void RegisterTypes()
    {
      IUnityContainer container = this;
      // Register NullChatCommunication as IChatCommunication
      container.RegisterType<IChatCommunication, NullChatCommunication>();
      // register MainWindowViewModel as MainWindowViewModel
      container.RegisterType<MainWindowViewModel>();
    }
    #endregion Singleton Management Region
  }

}
