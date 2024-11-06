using Prism.Mvvm;
using Reactive.Bindings;
using Slide.Models.Comparer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slide.Models
{
    public class SelectedFileComparer : BindableBase
    {
        private readonly Dictionary<string, FileComparerBase> dic = [];
        private readonly IObservable<DirectoryModel?> selectedDirectory;

        public ReactiveProperty<FileComparerBase> FileComparer { get; }

        public SelectedFileComparer(SelectedItemModel selectedItemModel)
        {
            this.selectedDirectory = selectedItemModel.SelectedDirectory;
            this.selectedDirectory.Subscribe(this.OnSelectedDirectoryChanged);
            this.FileComparer = new ReactiveProperty<FileComparerBase>(FilenameComparer.Instance);
            this.FileComparer.Subscribe(this.OnSetFileComparer);
        }

        private void OnSelectedDirectoryChanged(DirectoryModel? selectedDirectory)
        {
            if (selectedDirectory is null || selectedDirectory.DirectoryInfo.Value is not DirectoryInfo selectedDirectoryInfo) return;
            if (this.dic.TryGetValue(selectedDirectoryInfo.FullName, out FileComparerBase? selectedFileComparer))
            {
                this.FileComparer.Value = selectedFileComparer;
            }
            else
            {
                this.dic[selectedDirectoryInfo.FullName] = FilenameComparer.Instance;
                this.FileComparer.Value = FilenameComparer.Instance;
            }
        }

        private async void OnSetFileComparer(FileComparerBase fileComparer)
        {
            if (await this.selectedDirectory.LastOrDefaultAsync() is DirectoryModel selectedDirectory && selectedDirectory.DirectoryInfo.Value is DirectoryInfo selectedDirectoryInfo)
            {
                this.dic[selectedDirectoryInfo.FullName] = fileComparer;
            }
        }
    }
}
