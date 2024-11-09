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
using System.Reactive.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace Slide.ViewModels
{
    public class FileListBoxViewModel : BindableBase, IDisposable
    {
        private readonly IEventAggregator eventAggregator;
        private readonly SelectedItemModel selectedItemModel;
        private readonly SelectedFavoriteLevel favoriteLevel;

        private string? currentSelectedDirectoryPath = null;

        public ReactiveCommand<SelectionChangedEventArgs> SelectedItemChangedCommand { get; }

        // ReactiveCollectionでClearすると最初の要素で不都合が生じるため、ObservableCollectionで手続き的に処理
        public ObservableCollection<FileListBoxItemViewModel> Items { get; private set; } = [];

        public ListCollectionView ItemsView { get; private set; }

        public ReactivePropertySlim<int> SelectedIndex { get; }

        public FileListBoxViewModel(IEventAggregator eventAggregator, SelectedItemModel selectedItemModel, SelectedFavoriteLevel favoriteLevel)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<ClickPositionEvent>().Subscribe(this.HandleClickPositionEvent).AddTo(this.disposables);
            this.selectedItemModel = selectedItemModel;
            this.favoriteLevel = favoriteLevel;
            this.ItemsView = new ListCollectionView(this.Items) { IsLiveFiltering = true, IsLiveSorting = true };
            this.SelectedIndex = new ReactivePropertySlim<int>(-1).AddTo(this.disposables);
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
            if (directoryInfo?.FullName != this.currentSelectedDirectoryPath)
            {
                this.currentSelectedDirectoryPath = directoryInfo?.FullName;
                this.Items.Clear();
                if (directoryInfo is not null)
                {
                    try
                    {
                        this.Items.AddRange(directoryInfo.EnumerateFiles()
                            .AsParallel()
                            .AsOrdered()
                            .Where(fileInfo => Const.Extensions.Contains(fileInfo.Extension.ToLower()))
                            .Select(fileInfo => new FileListBoxItemViewModel(FileModel.Create(fileInfo)))
                        );
                    }
                    catch (IOException)
                    {
                        // Do nothing
                    }
                }
            }
            this.ItemsView.CustomSort = new FileListBoxItemViewModelComparer(fileComparer);
            this.ItemsView.Filter = vm => ((vm as FileListBoxItemViewModel)?.FavoriteLevel?.Value ?? -1) >= favoriteLevel;
        }

        private void OnSelectedItemChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is FileListBoxItemViewModel first)
            {
                this.selectedItemModel.SelectedFile.Value = first.FileModel;
            }
        }

        private void HandleClickPositionEvent(ClickPosition clickPosition)
        {
            if (clickPosition == ClickPosition.Middle) return;
            if (this.SelectedIndex.Value > -1)
            {
                var di = clickPosition switch
                {
                    ClickPosition.LeftQuarter => -1,
                    ClickPosition.RightQuarter => 1,
                    _ => 0
                };
                int count = this.ItemsView.Count;
                if (count < 0) throw new InvalidOperationException("count is negative");
                int newIndex = (this.SelectedIndex.Value + count + di) % count;
                this.SelectedIndex.Value = newIndex;
            }
            else if (!this.ItemsView.IsEmpty)
            {
                this.SelectedIndex.Value = 0;
            }
        }

        #region IDisposable
        private readonly System.Reactive.Disposables.CompositeDisposable disposables = new();
        public void Dispose() => this.disposables.Dispose();
        #endregion
    }
}
