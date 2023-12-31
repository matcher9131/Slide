using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Slide.Models;

namespace Slide.ViewModels
{
    public class ExplorerTreeViewItemViewModel : BindableBase, IDisposable
    {
        private readonly DirectoryModel directoryModel;

        public ReactivePropertySlim<string> DisplayText { get; }

        public ReadOnlyReactiveCollection<ExplorerTreeViewItemViewModel> Children { get; }

        public ExplorerTreeViewItemViewModel(DirectoryModel directoryModel)
        {
            this.directoryModel = directoryModel;
            this.DisplayText = this.directoryModel.Name.ToReactivePropertySlimAsSynchronized(x => x.Value).AddTo(this.disposables);
            this.Children = this.directoryModel.Children.ToReadOnlyReactiveCollection(x => new ExplorerTreeViewItemViewModel(x));
        }

        public void UpdateChildren() => this.directoryModel.UpdateChildren();

        #region IDisposable
        private readonly System.Reactive.Disposables.CompositeDisposable disposables = new();
        public void Dispose() => this.disposables.Dispose();
        #endregion
    }
}
