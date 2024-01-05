using Slide.ViewModels;
using System;
using System.Collections.Generic;
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

namespace Slide.Views
{
    /// <summary>
    /// ExplorerTreeView.xaml の相互作用ロジック
    /// </summary>
    public partial class ExplorerTreeView : UserControl
    {
        public ExplorerTreeView()
        {
            InitializeComponent();
            this.AddHandler(TreeViewItem.ExpandedEvent, new RoutedEventHandler(this.OnItemExpanded));
        }

        private void OnItemExpanded(object sender, RoutedEventArgs e)
        {
            var view = (TreeViewItem)e.OriginalSource;
            var viewModel = (ExplorerTreeViewItemViewModel)view.DataContext;
            viewModel.InitializeChildren();
        }
    }
}
