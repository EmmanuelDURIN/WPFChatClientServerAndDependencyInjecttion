using ChatBusinessLogic;
using ChatViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Registration;
using System.Composition.Convention;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using log4net.Core;
using log4net;
using System.Diagnostics;

namespace WPFChatClient
{
  public class WPFContainer : CompositionContainer
  {
    #region Singleton Management Region
    private WPFContainer(ComposablePartCatalog catalog, params ExportProvider[] providers)
      : base(catalog, providers)
    { }
    private static WPFContainer instance;
    public static WPFContainer Instance
    {
      get
      {
        if (instance == null)
        {
          instance = new WPFContainer(CreateCatalog());
        }
        return instance;
      }
    }
    private static object DefaultFactory(Type arg1, string arg2)
    {
      return null;
    }
    private static ComposablePartCatalog CreateCatalog()
    {
      var builder = new RegistrationBuilder();
      builder.ForType<NullChatCommunication>()
          .Export<IChatCommunication>()
          .SetCreationPolicy(CreationPolicy.Shared);

      builder.ForType<MainWindowViewModel>()
          .Export() 
          // équivalent à :
          // .Export<MainWindowViewModel>()
          // sauf que le type est implicite
          .ImportProperties<IChatCommunication>(p => p.Name == nameof(MainWindowViewModel.ChatCommunication))
          .SetCreationPolicy(CreationPolicy.NonShared);

      builder.ForType<LoggerFactory>()
          .Export()
          .ExportProperties<Func<Type, ILog>>(CheckLoggerFactoryProperty);

      builder.ForType<LoggerClient>()
          .Export()
          .ImportProperties<Func<Type,ILog>>(CheckLoggerFactoryName);
      
      AggregateCatalog aggregateCatalog = new AggregateCatalog();

      var catalog = new TypeCatalog(new[] { typeof(NullChatCommunication), typeof(MainWindowViewModel), typeof(LoggerClient), typeof(LoggerFactory) }, builder);

      aggregateCatalog.Catalogs.Add(catalog);
      //var catalog1 = new AssemblyCatalog(typeof(NullChatCommunication).Assembly);
      //var catalog2 = new AssemblyCatalog(typeof(MainWindowViewModel).Assembly);
      //var catalog3 = new AssemblyCatalog(typeof(LoggerClient).Assembly);
      //var catalog4 = new AssemblyCatalog(typeof(LoggerFactory).Assembly);
      ////, typeof(MainWindowViewModel), typeof(LoggerClient), typeof(LoggerFactory) }, builder);

      //aggregateCatalog.Catalogs.Add(catalog1);
      //aggregateCatalog.Catalogs.Add(catalog2);
      //aggregateCatalog.Catalogs.Add(catalog3);
      //aggregateCatalog.Catalogs.Add(catalog4);


      ////A essayer :
      //var conventionBuilder = new ConventionBuilder();
      //  .ForTypesMatching(AllTypes)
      //  .ImportProperties(CheckLoggerFactoryProperty);
      //var conventionCatalog = new TypeCatalog(new[] { typeof(NullChatCommunication), typeof(MainWindowViewModel), typeof(LoggerClient), typeof(LoggerFactory) }, conventionBuilder);

      //aggregateCatalog.Catalogs.Add(conventionCatalog);
      return aggregateCatalog;
    }

    private static bool AllTypes(Type inspectedType)
    {
      return true;
    }

    private static bool CheckLoggerFactoryProperty(PropertyInfo p)
    {
      return p.Name == "MakeLogger";
    }

    private static bool CheckLoggerFactoryName(PropertyInfo p)
    {
      return p.Name == "MakeLogger";
    }

    private static bool CheckPropName(PropertyInfo p)
    {
      return p.Name == "ViewModel";
    }
    #endregion Singleton Management Region
  }
}