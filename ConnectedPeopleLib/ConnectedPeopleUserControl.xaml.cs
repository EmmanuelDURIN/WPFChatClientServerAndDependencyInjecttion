using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConnectedPeopleLib
{
  /// <summary>
  /// Logique d'interaction pour ConnectedPeopleUserControl.xaml
  /// </summary>
  [Export("ConnectedPeopleUserControl")]
  [PartCreationPolicy(CreationPolicy.NonShared)]
  [RegionMemberLifetime(KeepAlive = false)]
  public partial class ConnectedPeopleUserControl : UserControl
  {
    private ConnectedPeopleViewModel viewModel;
    public ConnectedPeopleUserControl(ConnectedPeopleViewModel viewModel)
    {
      InitializeComponent();
      this.DataContext = this.viewModel = viewModel;
    }
  }
}
