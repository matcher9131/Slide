using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Data;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Slide.Models;
using Slide.Models.Comparer;

namespace Slide.ViewModels
{
    public class ExplorerTreeViewItemViewModel : BindableBase, IDisposable
    {
        #region Comparer
        private class ExplorerTreeViewItemFilenameComparer : IComparer
        {
            public int Compare(object? x, object? y)
            {
                return (x is ExplorerTreeViewItemViewModel vm1 && y is ExplorerTreeViewItemViewModel vm2)
                    ? FilenameComparer.Instance.Compare(vm1.DirectoryModel.DirectoryInfo.Value, vm2.DirectoryModel.DirectoryInfo.Value)
                    : 0;
            }

            public static ExplorerTreeViewItemFilenameComparer Instance = new();
        }

        private class ExplorerTreeViewItemLastWriteTimeComparer : IComparer
        {
            public int Compare(object? x, object? y)
            {
                return (x is ExplorerTreeViewItemViewModel vm1 && y is ExplorerTreeViewItemViewModel vm2)
                    ? LastWriteTimeComparer.Instance.Compare(vm1.DirectoryModel.DirectoryInfo.Value, vm2.DirectoryModel.DirectoryInfo.Value)
                    : 0;
            }

            public static ExplorerTreeViewItemLastWriteTimeComparer Instance = new();
        }

        private class ExplorerTreeViewItemCreationTimeComparer : IComparer
        {
            public int Compare(object? x, object? y)
            {
                return (x is ExplorerTreeViewItemViewModel vm1 && y is ExplorerTreeViewItemViewModel vm2)
                    ? CreationTimeComparer.Instance.Compare(vm1.DirectoryModel.DirectoryInfo.Value, vm2.DirectoryModel.DirectoryInfo.Value)
                    : 0;
            }

            public static ExplorerTreeViewItemCreationTimeComparer Instance = new();
        }
        #endregion

        public DirectoryModel DirectoryModel { get; }

        public ReadOnlyReactivePropertySlim<string> DisplayText { get; }

        public ReadOnlyReactiveCollection<ExplorerTreeViewItemViewModel> Children { get; }

        public ListCollectionView ChildrenView { get; }

        public ReactivePropertySlim<bool> IsSelected { get; }

        public ReactiveCommand OpenExplorerCommand { get; }

        public ExplorerTreeViewItemViewModel(DirectoryModel directoryModel)
        {
            this.DirectoryModel = directoryModel;
            this.DisplayText = this.DirectoryModel.Name.Select(x => x).ToReadOnlyReactivePropertySlim<string>().AddTo(this.disposables);
            this.Children = this.DirectoryModel.Children.ToReadOnlyReactiveCollection(child => new ExplorerTreeViewItemViewModel(child)).AddTo(this.disposables);
            this.ChildrenView = new ListCollectionView(this.Children) { IsLiveSorting = true };
            this.IsSelected = new ReactivePropertySlim<bool>().AddTo(this.disposables);
            this.OpenExplorerCommand = this.DirectoryModel.DirectoryInfo
                .Select(directoryInfo => directoryInfo?.FullName != null)
                .ToReactiveCommand()
                .WithSubscribe(this.OpenExplorer)
                .AddTo(this.disposables);
            this.DirectoryModel.FileComparer.Subscribe(this.OnComparerChanged).AddTo(this.disposables);
        }

        public void InitializeChildren() => this.DirectoryModel.InitializeChildren();

        private void OnComparerChanged(FileComparerBase comparer)
        {
            this.ChildrenView.CustomSort = new ExplorerTreeViewItemViewModelComparer(comparer);
        }

        private void OpenExplorer()
        {
            if (this.DirectoryModel.DirectoryInfo.Value is DirectoryInfo di)
            {
                System.Diagnostics.Process.Start("explorer.exe", di.FullName);
            }
        }

        #region IDisposable
        private readonly System.Reactive.Disposables.CompositeDisposable disposables = new();
        public void Dispose() => this.disposables.Dispose();
        #endregion
    }

    public class ExplorerTreeViewItemViewModelComparer(FileComparerBase comparer) : IComparer
    {
        private readonly FileComparerBase comparer = comparer;

        public int Compare(object? x, object? y)
        {
            return this.comparer.Compare((x as ExplorerTreeViewItemViewModel)?.DirectoryModel?.DirectoryInfo?.Value, (y as ExplorerTreeViewItemViewModel)?.DirectoryModel?.DirectoryInfo?.Value);
        }
    }

}
