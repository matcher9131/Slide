using Prism.Mvvm;
using Reactive.Bindings;
using Slide.Models.FileComparer;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace Slide.Models
{
    public class DirectoryModel : BindableBase
    {
        private bool childrenAreInitialized = false;

        public ReactivePropertySlim<DirectoryInfo?> DirectoryInfo { get; }

        public ReadOnlyReactivePropertySlim<string> Name { get; }

        public ReactiveCollection<DirectoryModel> Children { get; }

        public DirectoryModel(DirectoryInfo? directoryInfo)
        {
            this.DirectoryInfo = new ReactivePropertySlim<DirectoryInfo?>(directoryInfo);
            this.Name = this.DirectoryInfo.Select(x => x?.Name ?? "*").ToReadOnlyReactivePropertySlim<string>();
            this.Children = new();
            if (directoryInfo != null) // 無限ループ防止
            {
                this.Children.Add(new DirectoryModel(null));
            }
        }

        public void InitializeChildren()
        {
            if (this.childrenAreInitialized) return;
            this.Children.Clear();
            if (this.DirectoryInfo.Value is DirectoryInfo di)
            {
                //
                // TODO: ソート方法
                //
                this.Children.AddRange(di.EnumerateDirectories()
                    .AsParallel()
                    .AsOrdered()
                    .Where(childDirectoryInfo => !childDirectoryInfo.Attributes.HasFlag(FileAttributes.System))
                    .Select(childDirectoryInfo => new DirectoryModel(childDirectoryInfo))
                    .OrderBy(directoryModel => directoryModel.DirectoryInfo.Value, new FilenameComparer())
                );
            }
            this.childrenAreInitialized = true;
        }
    }
}
