//using Microsoft.Practices.Prism.MefExtensions;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.Composition;
//using System.ComponentModel.Composition.Hosting;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Windows;

//namespace WPFChatClient
//{
//  public class MefChatBootStrapper : MefBootstrapper
//  {
//    protected override void ConfigureAggregateCatalog()
//    {
//      base.ConfigureAggregateCatalog();
//      var executingAssembly = Assembly.GetExecutingAssembly();
//      this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(executingAssembly));
//      this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(ConnectedPeopleLib.PrismFacConnectedPeopleModule).Assembly));
//      //this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(RightModule.RightModule).Assembly));
//      //this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(MiddleModule.MiddleModule).Assembly));
//    }
//    protected override CompositionContainer CreateContainer()
//    {
//      var container = base.CreateContainer();
//      container.ComposeExportedValue(container);
//      return container;
//    }
//    protected override DependencyObject CreateShell()
//    {
//      return Container.GetExportedValue<MainWindow>();
//      //return new MainWindow(new ChatViewModel.MainWindowViewModel());
//    }
//    protected override void InitializeShell()
//    {
//      base.InitializeShell();

//      Application.Current.MainWindow = (MainWindow)this.Shell;
//      Application.Current.MainWindow.Show();
//    }
//  }
//}
