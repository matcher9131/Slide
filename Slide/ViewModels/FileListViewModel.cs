using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Slide.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Slide.ViewModels
{
    public class FileListViewModel : BindableBase, IDisposable
    {
        private readonly SelectedItemModel selectedItemModel;

        // ItemsをクリアするためのSubject
        // private readonly Subject<Unit> clearSubject = new();

        public ReactiveCommand<SelectionChangedEventArgs> SelectedItemChangedCommand { get; }

        public ReadOnlyReactiveCollection<FileListViewItemViewModel> Items { get; }

        public FileListViewModel(SelectedItemModel selectedItemModel)
        {
            this.selectedItemModel = selectedItemModel;
            this.SelectedItemChangedCommand = new ReactiveCommand<SelectionChangedEventArgs>().WithSubscribe(this.OnSelectedItemChanged).AddTo(this.disposables);
            this.Items = this.selectedItemModel.SelectedDirectory.SelectMany(selectedDirectory =>
            {
                if (selectedDirectory == null) return Enumerable.Empty<FileListViewItemViewModel>();
                try
                {
                    return selectedDirectory.EnumerateFiles()
                    .AsParallel()
                    .AsOrdered()
                    .Where(fileInfo => Const.Extensions.Contains(fileInfo.Extension))
                    .OrderBy(fileInfo => fileInfo.Name, new FilenameComparer())
                    .Select(fileInfo => new FileListViewItemViewModel(new FileModel(fileInfo)));
                }
                catch (IOException)
                {
                    return Enumerable.Empty<FileListViewItemViewModel>();
                }
            }).ToReadOnlyReactiveCollection(this.selectedItemModel.SelectedDirectory.Select(_ => Unit.Default));
        }

        private void OnSelectedItemChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is FileListViewItemViewModel vm)
            {
                this.selectedItemModel.SelectedFile.Value = vm.FileInfo;
            }
        }

        #region IDisposable
        private readonly System.Reactive.Disposables.CompositeDisposable disposables = new();
        public void Dispose() => this.disposables.Dispose();
        #endregion
    }
}
