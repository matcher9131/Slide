using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Slide.Models.Comparer;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Slide.Models
{
    public class SelectedItemModel : BindableBase
    {
        private DirectoryModel? selectedDirectory = null;
        private IObservable<FileComparerBase>? selectedDirectoryComparer = null;
        private IDisposable? selectedDirectoryComparerUnsubscriber = null;

        public DirectoryModel? SelectedDirectory
        {
            get => this.selectedDirectory;
            set
            {
                this.selectedDirectoryComparerUnsubscriber?.Dispose();
                if (value is not null)
                {
                    this.selectedDirectoryComparer = value.FileComparer;
                    this.selectedDirectoryComparerUnsubscriber = this.selectedDirectoryComparer.Subscribe(this.OnFileComparerChanged);
                    this.SelectedDirectoryAndComparer.OnNext((value.DirectoryInfo?.Value, value.FileComparer.Value));
                }
                this.selectedDirectory = value;
            }
        }

        public Subject<(DirectoryInfo? directoryInfo, FileComparerBase comparer)> SelectedDirectoryAndComparer { get; } = new();

        private void OnFileComparerChanged(FileComparerBase comparer)
        {
            this.SelectedDirectoryAndComparer.OnNext((this.selectedDirectory?.DirectoryInfo?.Value, comparer));
        }

        public ReactivePropertySlim<FileModel?> SelectedFile { get; }

        public SelectedItemModel()
        {
            this.SelectedFile = new ReactivePropertySlim<FileModel?>(null);
        }
    }
}
