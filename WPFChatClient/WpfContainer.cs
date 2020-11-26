using Autofac;
using ChatBusinessLogic;
using ChatViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfChatClient
{
  class WpfContainer
  {
    internal static IContainer CreateContainer()
    {
      var builder = new ContainerBuilder();

      // ... TODO : s’inspirer du slide AutoFac : enregistrement des types

      builder.RegisterType<MainWindowViewModel>();
      builder.RegisterType<NullChatCommunication>().As<IChatCommunication>();
      IContainer container = builder.Build();

      return container;
    }

  }
}
