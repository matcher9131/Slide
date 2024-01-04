using Prism.Mvvm;
using Reactive.Bindings;
using System.IO;

namespace Slide.Models
{
    public class SelectedItemModel : BindableBase
    {
        public ReactivePropertySlim<DirectoryInfo?> SelectedDirectory { get; }

        public ReactivePropertySlim<FileInfo?> SelectedFile { get; }

        public SelectedItemModel()
        {
            this.SelectedDirectory = new ReactivePropertySlim<DirectoryInfo?>(null);
            this.SelectedFile = new ReactivePropertySlim<FileInfo?>(null);
        }
    }
}
