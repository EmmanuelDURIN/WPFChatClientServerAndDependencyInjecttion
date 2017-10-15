using Autofac;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using System.ComponentModel.Composition;

namespace ConnectedPeopleLib
{
  [ModuleExport(typeof(PrismConnectedPeopleModule))]
  //[Module]
  public class PrismConnectedPeopleModule : IModule
  {
    private IRegionManager regionManager;
    [ImportingConstructor]
    public PrismConnectedPeopleModule(IRegionManager regionManager)
    {
      this.regionManager = regionManager;
    }
    public void Initialize()
    {
      regionManager.RegisterViewWithRegion("ToolsRegion", typeof(ConnectedPeopleUserControl));
    }
  }
}