using Reactive.Bindings;
using Slide.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Reactive.Bindings.Extensions;
using Prism.Mvvm;
using System.IO;

namespace Slide.ViewModels
{
    public class FileListBoxItemViewModel : BindableBase, IDisposable
    {
        private readonly FileModel fileModel;

        public ReadOnlyReactivePropertySlim<string> DisplayText { get; }

        public ReactivePropertySlim<bool> IsSelected { get; }

        public ReadOnlyReactivePropertySlim<int> FavoriteLevel { get; }

        public ReactiveCommand IncrementFavoriteLevelCommand { get; }
        public ReactiveCommand OpenExplorerCommand { get; }

        public FileListBoxItemViewModel(FileModel fileModel)
        {
            this.fileModel = fileModel;
            this.DisplayText = this.fileModel.Name.Select(x => x).ToReadOnlyReactivePropertySlim<string>().AddTo(this.disposables);
            this.IsSelected = new ReactivePropertySlim<bool>().AddTo(this.disposables);
            this.FavoriteLevel = this.fileModel.FavoriteLevel.Select(x => x).ToReadOnlyReactivePropertySlim().AddTo(this.disposables);
            this.IncrementFavoriteLevelCommand = new ReactiveCommand().WithSubscribe(this.IncrementFavoriteLevel).AddTo(this.disposables);
            this.OpenExplorerCommand = new ReactiveCommand().WithSubscribe(this.OpenExplorer).AddTo(this.disposables);
        }

        public FileInfo FileInfo => this.fileModel.FileInfo.Value;

        public void IncrementFavoriteLevel()
        {
            this.fileModel.FavoriteLevel.Value = this.fileModel.FavoriteLevel.Value switch
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
            System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{this.fileModel.FileInfo.Value.FullName}\"");
        }

        #region IDisposable
        private readonly System.Reactive.Disposables.CompositeDisposable disposables = new();
        public void Dispose() => this.disposables.Dispose();
        #endregion
    }
}
