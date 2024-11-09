using Reactive.Bindings;
using Slide.Models;
using System;
using System.Linq;
using System.Reactive.Linq;
using Reactive.Bindings.Extensions;
using Prism.Mvvm;
using System.Collections;
using Slide.Models.Comparer;

namespace Slide.ViewModels
{
    public class FileListBoxItemViewModel : BindableBase, IDisposable
    {
        public FileModel FileModel { get; }

        public ReadOnlyReactivePropertySlim<string> DisplayText { get; }

        public ReadOnlyReactivePropertySlim<int> FavoriteLevel { get; }

        public ReactiveCommand IncrementFavoriteLevelCommand { get; }
        public ReactiveCommand OpenExplorerCommand { get; }

        public FileListBoxItemViewModel(FileModel fileModel)
        {
            this.FileModel = fileModel;
            this.DisplayText = this.FileModel.Name.Select(x => x).ToReadOnlyReactivePropertySlim<string>().AddTo(this.disposables);
            this.FavoriteLevel = this.FileModel.FavoriteLevel.Select(x => x).ToReadOnlyReactivePropertySlim().AddTo(this.disposables);
            this.IncrementFavoriteLevelCommand = new ReactiveCommand().WithSubscribe(this.IncrementFavoriteLevel).AddTo(this.disposables);
            this.OpenExplorerCommand = new ReactiveCommand().WithSubscribe(this.OpenExplorer).AddTo(this.disposables);
        }
        public void IncrementFavoriteLevel()
        {
            this.FileModel.FavoriteLevel.Value = this.FileModel.FavoriteLevel.Value switch
            {
                0 => 1,
                1 => 2,
                2 => 3,
                3 => 0,
                _ => 0
            };
        }

        public void OpenExplorer()
        {
            System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{this.FileModel.FileInfo.Value.FullName}\"");
        }

        #region IDisposable
        private readonly System.Reactive.Disposables.CompositeDisposable disposables = new();
        public void Dispose() => this.disposables.Dispose();
        #endregion
    }

    public class FileListBoxItemViewModelComparer(FileComparerBase comparer) : IComparer
    {
        private readonly FileComparerBase comparer = comparer;

        public int Compare(object? x, object? y)
        {
            return this.comparer.Compare((x as FileListBoxItemViewModel)?.FileModel?.FileInfo?.Value, (y as FileListBoxItemViewModel)?.FileModel?.FileInfo?.Value);
        }
    }
}
