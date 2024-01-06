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
    public class FileListBoxViewModel : BindableBase, IDisposable
    {
        private readonly SelectedItemModel selectedItemModel;

        // ItemsをクリアするためのSubject
        // private readonly Subject<Unit> clearSubject = new();

        public ReactiveCommand<SelectionChangedEventArgs> SelectedItemChangedCommand { get; }

        public ReadOnlyReactiveCollection<FileListBoxItemViewModel> Items { get; }

        public FileListBoxViewModel(SelectedItemModel selectedItemModel)
        {
            this.selectedItemModel = selectedItemModel;
            this.SelectedItemChangedCommand = new ReactiveCommand<SelectionChangedEventArgs>().WithSubscribe(this.OnSelectedItemChanged).AddTo(this.disposables);
            this.Items = this.selectedItemModel.SelectedDirectory.SelectMany(selectedDirectory =>
            {
                if (selectedDirectory == null) return Enumerable.Empty<FileListBoxItemViewModel>();
                try
                {
                    return selectedDirectory.EnumerateFiles()
                    .AsParallel()
                    .AsOrdered()
                    .Where(fileInfo => Const.Extensions.Contains(fileInfo.Extension))
                    .OrderBy(fileInfo => fileInfo.Name, new FilenameComparer())
                    .Select(fileInfo => new FileListBoxItemViewModel(new FileModel(fileInfo)));
                }
                catch (IOException)
                {
                    return Enumerable.Empty<FileListBoxItemViewModel>();
                }
            }).ToReadOnlyReactiveCollection(this.selectedItemModel.SelectedDirectory.Select(_ => Unit.Default));
        }

        private void OnSelectedItemChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is FileListBoxItemViewModel vm)
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
