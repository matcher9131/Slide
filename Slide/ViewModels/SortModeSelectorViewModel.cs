using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Slide.Models;
using System;
using System.Reactive.Linq;
using System.Windows.Media;

namespace Slide.ViewModels
{
    public class SortModeSelectorViewModel : BindableBase, IDisposable
    {
        private readonly ReactivePropertySlim<SortMode> selectedSortMode;

        public ReadOnlyReactivePropertySlim<Color> FilenameButtonColor { get; }
        public ReadOnlyReactivePropertySlim<Color> LastWriteTimeButtonColor { get; }
        public ReadOnlyReactivePropertySlim<Color> CreationTimeButtonColor { get; }


        public ReactiveCommandSlim<SortMode> ClickCommand { get; }

        public SortModeSelectorViewModel(SelectedSortMode selectedSortMode, bool isForDirectory)
        {
            this.selectedSortMode = isForDirectory ? selectedSortMode.DirectorySortMode : selectedSortMode.FileSortMode;
            this.FilenameButtonColor = this.selectedSortMode
                .Select(sortMode => sortMode == SortMode.Filename ? Colors.AntiqueWhite : Colors.Black)
                .ToReadOnlyReactivePropertySlim()
                .AddTo(this.disposables);
            this.LastWriteTimeButtonColor = this.selectedSortMode
                .Select(sortMode => sortMode == SortMode.LastWriteTime ? Colors.AntiqueWhite : Colors.Black)
                .ToReadOnlyReactivePropertySlim()
                .AddTo(this.disposables);
            this.CreationTimeButtonColor = this.selectedSortMode
                .Select(sortMode => sortMode == SortMode.CreationTime ? Colors.AntiqueWhite : Colors.Black)
                .ToReadOnlyReactivePropertySlim()
                .AddTo(this.disposables);
            this.ClickCommand = new ReactiveCommandSlim<SortMode>().WithSubscribe(this.SetSortMode).AddTo(this.disposables);
        }

        private void SetSortMode(SortMode sortMode)
        {
            if (this.selectedSortMode.Value != sortMode)
            {
                this.selectedSortMode.Value = sortMode;
            }
        }

        #region IDisposable
        private readonly System.Reactive.Disposables.CompositeDisposable disposables = [];
        public void Dispose() => this.disposables.Dispose();
        #endregion
    }
}
