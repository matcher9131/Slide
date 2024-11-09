using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Slide.Models;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace Slide.ViewModels
{
    public class ExplorerTreeViewModel : BindableBase, IDisposable
    {
        private readonly SelectedItemModel selectedItemModel;

        public ReactiveCommand<RoutedPropertyChangedEventArgs<object>> SelectedItemChangedCommand { get; }

        public ReactiveCollection<ExplorerTreeViewItemViewModel> Children { get; }

        public ExplorerTreeViewModel(SelectedItemModel selectedItemModel)
        {
            this.selectedItemModel = selectedItemModel;
            this.SelectedItemChangedCommand = new ReactiveCommand<RoutedPropertyChangedEventArgs<object>>().WithSubscribe(this.OnSelectedItemChanged).AddTo(this.disposables);
            this.Children = new ReactiveCollection<ExplorerTreeViewItemViewModel>().AddTo(this.disposables);

            var directories = DriveInfo.GetDrives().Select(drive => drive.RootDirectory);
            foreach (var d in directories)
            {
                this.Children.Add(new ExplorerTreeViewItemViewModel(new DirectoryModel(d)));
            }
        }

        private void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is ExplorerTreeViewItemViewModel vm)
            {
                this.selectedItemModel.SelectedFile.Value = null;
                this.selectedItemModel.SelectedDirectory = vm.DirectoryModel;
                e.Handled = true;
            }
        }

        #region IDisposable
        private readonly System.Reactive.Disposables.CompositeDisposable disposables = new();
        public void Dispose() => this.disposables.Dispose();
        #endregion
    }
}
