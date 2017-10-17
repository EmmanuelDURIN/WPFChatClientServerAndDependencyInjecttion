using Autofac;
using ChatBusinessLogic;
using ChatViewModel;
using ConnectedPeopleLib;
using Microsoft.Practices.Prism.Modularity;
using Prism.AutofacExtension;
using SignalRChatClient;
using System;
using System.Windows;
using TechnicalService;

namespace WPFChatClient
{
  public class AutofacChatBootstrapper : AutofacBootstrapper
  {
    protected override void ConfigureContainer(ContainerBuilder builder)
    {
      base.ConfigureContainer(builder);
      builder.RegisterType<MainWindow>();
      builder.RegisterInstance(new ChatClient())
          .As<INotifyingClientChatCommunication>();
      builder.RegisterType<MainWindowViewModel>();
      builder.RegisterModule<LoggingModule>();
      // Register autofac module
      builder.RegisterModule<AutoFacConnectedPeopleModule>();
    }
    protected override void ConfigureModuleCatalog()
    {
      base.ConfigureModuleCatalog();
      // Register prism module
      Type typeConnectedPeopleModule = typeof(PrismConnectedPeopleModule);
      ModuleCatalog.AddModule(new ModuleInfo(typeConnectedPeopleModule.Name, typeConnectedPeopleModule.AssemblyQualifiedName));
      // TODO
      //ModuleCatalog = new DirectoryModuleCatalog() { ModulePath = Environment.CurrentDirectory};
    }
    protected override DependencyObject CreateShell()
    {
      return Container.Resolve<MainWindow>();
    }
    protected override void InitializeShell()
    {
      base.InitializeShell();

      Application.Current.MainWindow = (MainWindow)this.Shell;
      Application.Current.MainWindow.Show();
    }
  }
}