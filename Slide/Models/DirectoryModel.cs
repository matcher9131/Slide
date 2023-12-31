using Prism.Mvvm;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace Slide.Models
{
    public class DirectoryModel : BindableBase
    {
        public ReactivePropertySlim<DirectoryInfo?> DirectoryInfo { get; }

        public ReadOnlyReactivePropertySlim<string> Name { get; }

        public ReactiveCollection<DirectoryModel> Children { get; }

        public DirectoryModel(DirectoryInfo? directoryInfo)
        {
            this.DirectoryInfo = new ReactivePropertySlim<DirectoryInfo?>(directoryInfo);
            this.Name = this.DirectoryInfo.Select(x => x?.Name ?? "").ToReadOnlyReactivePropertySlim<string>();
            this.Children = new();
        }

        public void UpdateChildren()
        {
            this.Children.Clear();
            if (this.DirectoryInfo.Value is DirectoryInfo di)
            {
                this.Children.AddRange(di.EnumerateDirectories().Select(directoryInfo => new DirectoryModel(directoryInfo)));
            }
            
        }
    }
}
