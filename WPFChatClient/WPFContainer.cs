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
using TechnicalService;

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
          instance = new WPFContainer(CreateCatalogWithMEF2());
        }
        return instance;
      }
    }
    private static object DefaultFactory(Type arg1, string arg2)
    {
      return null;
    }
    private static ComposablePartCatalog CreateCatalogWithMEF2()
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

      Predicate<PropertyInfo> propNameCondition = p => p.Name == nameof(LoggerFactory.MakeLogger);
      builder.ForType<LoggerFactory>()
          .Export()
          .ExportProperties<Func<Type, ILog>>( propNameCondition );

      builder.ForTypesMatching( type => type.Name.ToLower().EndsWith("viewmodel"))
          .ImportProperties<Func<Type, ILog>>(propNameCondition);

      AggregateCatalog aggregateCatalog = new AggregateCatalog();

      var nullCommLibCatalog = new AssemblyCatalog(typeof(NullChatCommunication).Assembly, builder);
      var viewmodelCatalog = new AssemblyCatalog(typeof(MainWindowViewModel).Assembly, builder);
      var technicalServiceCatalog = new AssemblyCatalog(typeof(LoggerFactory).Assembly, builder);

      aggregateCatalog.Catalogs.Add(nullCommLibCatalog);
      aggregateCatalog.Catalogs.Add(viewmodelCatalog);
      aggregateCatalog.Catalogs.Add(technicalServiceCatalog);

      return aggregateCatalog;
    }
    #endregion Singleton Management Region
  }
}