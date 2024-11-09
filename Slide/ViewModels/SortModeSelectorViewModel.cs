using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Slide.Models;
using Slide.Models.Comparer;
using System;
using System.Reactive.Linq;
using System.Windows.Media;

namespace Slide.ViewModels
{
    public class SortModeSelectorViewModel : BindableBase, IDisposable
    {
        private readonly SelectedItemModel selectedItemModel;

        public ReadOnlyReactivePropertySlim<FileComparerBase?> SelectedFileComparer { get; }

        public ReadOnlyReactivePropertySlim<Color> FilenameButtonColor { get; }
        public ReadOnlyReactivePropertySlim<Color> LastWriteTimeButtonColor { get; }
        public ReadOnlyReactivePropertySlim<Color> CreationTimeButtonColor { get; }


        public ReactiveCommandSlim ClickFilenameButtonCommand { get; }
        public ReactiveCommandSlim ClickLastWriteTimeButtonCommand { get; }
        public ReactiveCommandSlim ClickCreationTimeButtonCommand { get; }

        public SortModeSelectorViewModel(SelectedItemModel selectedItemModel)
        {
            this.selectedItemModel = selectedItemModel;
            this.SelectedFileComparer = this.selectedItemModel.SelectedDirectoryAndComparer.Select(tuple => tuple.comparer).ToReadOnlyReactivePropertySlim();
            this.FilenameButtonColor = this.SelectedFileComparer
                .Select(selectedFileComparer => selectedFileComparer == FilenameComparer.Instance ? Colors.AntiqueWhite : Colors.Black)
                .ToReadOnlyReactivePropertySlim()
                .AddTo(this.disposables);
            this.LastWriteTimeButtonColor = this.SelectedFileComparer
                .Select(selectedFileComparer => selectedFileComparer == LastWriteTimeComparer.Instance ? Colors.AntiqueWhite : Colors.Black)
                .ToReadOnlyReactivePropertySlim()
                .AddTo(this.disposables);
            this.CreationTimeButtonColor = this.SelectedFileComparer
                .Select(selectedFileComparer => selectedFileComparer == CreationTimeComparer.Instance ? Colors.AntiqueWhite : Colors.Black)
                .ToReadOnlyReactivePropertySlim()
                .AddTo(this.disposables);
            this.ClickFilenameButtonCommand = new ReactiveCommandSlim().WithSubscribe(this.SetFilenameComparer).AddTo(this.disposables);
            this.ClickLastWriteTimeButtonCommand = new ReactiveCommandSlim().WithSubscribe(this.SetLastWriteTimeComparer).AddTo(this.disposables);
            this.ClickCreationTimeButtonCommand = new ReactiveCommandSlim().WithSubscribe(this.SetCreationTimeComparer).AddTo(this.disposables);
        }

        private void SetFilenameComparer()
        {
            if (this.selectedItemModel.SelectedDirectory is not null && this.SelectedFileComparer.Value != FilenameComparer.Instance)
            {
                this.selectedItemModel.SelectedDirectory.FileComparer.Value = FilenameComparer.Instance;
            }
        }

        private void SetLastWriteTimeComparer()
        {
            if (this.selectedItemModel.SelectedDirectory is not null && this.SelectedFileComparer.Value != LastWriteTimeComparer.Instance)
            {
                this.selectedItemModel.SelectedDirectory.FileComparer.Value = LastWriteTimeComparer.Instance;
            }
        }

        private void SetCreationTimeComparer()
        {
            if (this.selectedItemModel.SelectedDirectory is not null && this.SelectedFileComparer.Value != CreationTimeComparer.Instance)
            {
                this.selectedItemModel.SelectedDirectory.FileComparer.Value = CreationTimeComparer.Instance;
            }
        }

        #region IDisposable
        private readonly System.Reactive.Disposables.CompositeDisposable disposables = [];
        public void Dispose() => this.disposables.Dispose();
        #endregion
    }
}
