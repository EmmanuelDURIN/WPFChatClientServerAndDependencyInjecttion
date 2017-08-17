using ChatViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Text;

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
      AggregateCatalog aggregateCatalog = new AggregateCatalog();
      // Chargement de toutes les DLL dans le répertoire courant
      aggregateCatalog.Catalogs.Add(new DirectoryCatalog(path: @".\", searchPattern: "*.dll"));
      return aggregateCatalog;
    }
    #endregion Singleton Management Region
  }
}