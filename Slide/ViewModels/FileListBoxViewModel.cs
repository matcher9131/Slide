using Prism.Events;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Slide.Behavior;
using Slide.Models;
using Slide.Models.Comparer;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Controls;

namespace Slide.ViewModels
{
    public class FileListBoxViewModel : BindableBase, IDisposable
    {
        private readonly IEventAggregator eventAggregator;
        private readonly SelectedItemModel selectedItemModel;
        private readonly SelectedFavoriteLevel favoriteLevel;

        // ItemsをクリアするためのSubject
        private readonly Subject<Unit> clearSubject = new();

        public ReactiveCommand<SelectionChangedEventArgs> SelectedItemChangedCommand { get; }

        // ReactiveCollectionでClearすると最初の要素で不都合が生じるため、ObservableCollectionで手続き的に処理
        public ObservableCollection<FileListBoxItemViewModel> Items { get; private set; } = [];

        public FileListBoxViewModel(IEventAggregator eventAggregator, SelectedItemModel selectedItemModel, SelectedFavoriteLevel favoriteLevel)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<ClickPositionEvent>().Subscribe(this.HandleClickPositionEvent).AddTo(this.disposables);
            this.selectedItemModel = selectedItemModel;
            this.favoriteLevel = favoriteLevel;
            this.SelectedItemChangedCommand = new ReactiveCommand<SelectionChangedEventArgs>().WithSubscribe(this.OnSelectedItemChanged).AddTo(this.disposables);
            Observable.CombineLatest(
                this.selectedItemModel.SelectedDirectoryAndComparer,
                this.favoriteLevel.Level,
                Tuple.Create
            ).Subscribe(tuple =>
            {
                var ((selectedDirectoryInfo, fileComparer), favoriteLevel) = tuple;
                this.SetItems(selectedDirectoryInfo, fileComparer, favoriteLevel);
            }).AddTo(this.disposables);
        }

        private void SetItems(DirectoryInfo? directoryInfo, FileComparerBase fileComparer, int favoriteLevel)
        {
            this.Items.Clear();
            if (directoryInfo is null) return;
            try
            {
                this.Items.AddRange(directoryInfo.EnumerateFiles()
                    .AsParallel()
                    .AsOrdered()
                    .Where(fileInfo => Const.Extensions.Contains(fileInfo.Extension.ToLower()))
                    .Select(fileInfo => new FileListBoxItemViewModel(FileModel.Create(fileInfo)))
                    .Where(vm => vm.FavoriteLevel.Value >= favoriteLevel)
                    .OrderBy(vm => vm.FileModel.FileInfo.Value, fileComparer)
                );
            }
            catch (IOException)
            {
                // Do nothing
            }
        }

        private void OnSelectedItemChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is FileListBoxItemViewModel first)
            {
                this.selectedItemModel.SelectedFile.Value = first.FileModel;
                first.IsSelected.Value = true;
            }

            foreach (var item in e.RemovedItems)
            {
                if (item is FileListBoxItemViewModel vm)
                {
                    vm.IsSelected.Value = false;
                }
            }
        }

        private void HandleClickPositionEvent(ClickPosition clickPosition)
        {
            if (clickPosition == ClickPosition.Middle) return;
            var selectedFileListBoxItemViewModel = this.Items.FirstOrDefault(item => item.IsSelected.Value);
            if (selectedFileListBoxItemViewModel != null)
            {
                // 選択状態のItemがあるときは、そのItemの選択状態を解除してその次or前のItemを選択状態にする
                var currentIndex = this.Items.IndexOf(selectedFileListBoxItemViewModel);
                var di = clickPosition switch
                {
                    ClickPosition.LeftQuarter => -1,
                    ClickPosition.RightQuarter => 1,
                    _ => 0
                };
                var newIndex = (currentIndex + this.Items.Count + di) % this.Items.Count;
                this.Items[newIndex].IsSelected.Value = true;
                if (selectedFileListBoxItemViewModel != this.Items[newIndex])
                {
                    selectedFileListBoxItemViewModel.IsSelected.Value = false;
                }
            }
            else
            {
                // 選択状態のItemが無いときは最初のItemを選択状態にする
                if (this.Items.FirstOrDefault() is FileListBoxItemViewModel vm)
                {
                    vm.IsSelected.Value = true;
                }
            }
        }

        #region IDisposable
        private readonly System.Reactive.Disposables.CompositeDisposable disposables = new();
        public void Dispose() => this.disposables.Dispose();
        #endregion
    }
}
