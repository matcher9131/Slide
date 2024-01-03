using System;
using System.Linq;
using System.Reactive.Linq;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Slide.Models;

namespace Slide.ViewModels
{
    public class ExplorerTreeViewItemViewModel : BindableBase, IDisposable
    {
        private readonly DirectoryModel directoryModel;

        public ReadOnlyReactivePropertySlim<string> DisplayText { get; }

        public ReadOnlyReactiveCollection<ExplorerTreeViewItemViewModel> Children { get; }

        public ReactivePropertySlim<bool> IsSelected { get; }

        public ExplorerTreeViewItemViewModel(DirectoryModel directoryModel)
        {
            this.directoryModel = directoryModel;
            this.DisplayText = this.directoryModel.Name.Select(x => x).ToReadOnlyReactivePropertySlim<string>();
            this.Children = this.directoryModel.Children.ToReadOnlyReactiveCollection(x => new ExplorerTreeViewItemViewModel(x)).AddTo(this.disposables);
            this.IsSelected = new ReactivePropertySlim<bool>().AddTo(this.disposables);
        }

        public string? DirectoryFullName => this.directoryModel.DirectoryInfo.Value?.FullName;

        public void UpdateChildren() => this.directoryModel.InitializeChildren();

        #region IDisposable
        private readonly System.Reactive.Disposables.CompositeDisposable disposables = new();
        public void Dispose() => this.disposables.Dispose();
        #endregion
    }
}
