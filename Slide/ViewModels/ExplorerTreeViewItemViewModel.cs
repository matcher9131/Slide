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

        public ReactiveCommand OpenExplorerCommand { get; }

        public ExplorerTreeViewItemViewModel(DirectoryModel directoryModel)
        {
            this.directoryModel = directoryModel;
            this.DisplayText = this.directoryModel.Name.Select(x => x).ToReadOnlyReactivePropertySlim<string>().AddTo(this.disposables);
            this.Children = this.directoryModel.Children.ToReadOnlyReactiveCollection(child => new ExplorerTreeViewItemViewModel(child)).AddTo(this.disposables);
            this.IsSelected = new ReactivePropertySlim<bool>().AddTo(this.disposables);
            this.OpenExplorerCommand = this.directoryModel.DirectoryInfo
                .Select(directoryInfo => directoryInfo?.FullName != null)
                .ToReactiveCommand()
                .WithSubscribe(this.OpenExplorer)
                .AddTo(this.disposables);
        }

        public DirectoryInfo? DirectoryInfo => this.directoryModel.DirectoryInfo.Value;

        public void InitializeChildren() => this.directoryModel.InitializeChildren();

        public void OpenExplorer()
        {
            if (this.directoryModel.DirectoryInfo.Value is DirectoryInfo di)
            {
                System.Diagnostics.Process.Start("explorer.exe", di.FullName);
            }
        }

        #region IDisposable
        private readonly System.Reactive.Disposables.CompositeDisposable disposables = new();
        public void Dispose() => this.disposables.Dispose();
        #endregion
    }
}
