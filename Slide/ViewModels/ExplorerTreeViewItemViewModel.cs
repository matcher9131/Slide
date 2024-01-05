using System;
using System.IO;
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
            this.DisplayText = this.directoryModel.Name.Select(x => x).ToReadOnlyReactivePropertySlim<string>().AddTo(this.disposables);
            this.Children = this.directoryModel.Children.ToReadOnlyReactiveCollection(child => new ExplorerTreeViewItemViewModel(child)).AddTo(this.disposables);
            this.IsSelected = new ReactivePropertySlim<bool>().AddTo(this.disposables);
        }

        public DirectoryInfo? DirectoryInfo => this.directoryModel.DirectoryInfo.Value;

        public void InitializeChildren() => this.directoryModel.InitializeChildren();

        #region IDisposable
        private readonly System.Reactive.Disposables.CompositeDisposable disposables = new();
        public void Dispose() => this.disposables.Dispose();
        #endregion
    }
}
