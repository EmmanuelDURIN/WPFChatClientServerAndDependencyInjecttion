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

namespace WPFChatClient
{
  public class WPFContainer : CompositionContainer
  {
    #region Singleton Management Region
    private WPFContainer(ComposablePartCatalog catalog)
      : base(catalog)
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
    private static ComposablePartCatalog CreateCatalog()
    {
      var builder = new RegistrationBuilder();
      builder.ForType<NullChatCommunication>()
          .Export<IChatCommunication>()
          .SetCreationPolicy(CreationPolicy.Shared);

      builder.ForType<MainWindowViewModel>()
          .Export<MainWindowViewModel>() 
          .ImportProperties<IChatCommunication>(p => p.Name == nameof(MainWindowViewModel.ChatCommunication))
          .SetCreationPolicy(CreationPolicy.NonShared);

      AggregateCatalog aggregateCatalog = new AggregateCatalog();

      var catalog = new TypeCatalog(new[] { typeof(NullChatCommunication), typeof(MainWindowViewModel) }, builder);
      aggregateCatalog.Catalogs.Add(catalog);
      return aggregateCatalog;
    }

    private static bool CheckPropName(PropertyInfo p)
    {
      return p.Name == "ViewModel";
    }
    #endregion Singleton Management Region
  }
}