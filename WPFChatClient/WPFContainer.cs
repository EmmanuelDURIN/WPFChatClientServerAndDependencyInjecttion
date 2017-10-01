using ChatBusinessLogic;
using ChatViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using log4net.Core;
using log4net;
using System.Diagnostics;
using Autofac;
using TechnicalService;
using SignalRChatClient;

namespace WPFChatClient
{
  public class WPFContainer
  {
    public static IContainer CreateContainer()
    {
      var builder = new ContainerBuilder();
      builder.RegisterInstance(new ChatClient())
             .As<IClientChatCommunication>();
      builder.RegisterType<MainWindowViewModel>();
      builder.RegisterModule<LoggingModule>();
      IContainer container = builder.Build();
      return container;
    }
  }
}