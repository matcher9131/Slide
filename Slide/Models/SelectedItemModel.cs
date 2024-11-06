using Prism.Mvvm;
using Reactive.Bindings;
using System.IO;

namespace Slide.Models
{
    public class SelectedItemModel : BindableBase
    {
        public ReactivePropertySlim<DirectoryModel?> SelectedDirectory { get; }

        public ReactivePropertySlim<FileModel?> SelectedFile { get; }

        public SelectedItemModel()
        {
            this.SelectedDirectory = new ReactivePropertySlim<DirectoryModel?>(null);
            this.SelectedFile = new ReactivePropertySlim<FileModel?>(null);
        }
    }
}
