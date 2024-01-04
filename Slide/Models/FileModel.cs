using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slide.Models
{
    public class FileModel : BindableBase
    {
        public ReactivePropertySlim<FileInfo> FileInfo { get; }

        public ReadOnlyReactivePropertySlim<string> Name { get; }

        public ReadOnlyReactivePropertySlim<string> FullName { get; }

        public FileModel(FileInfo fileInfo)
        {
            this.FileInfo = new ReactivePropertySlim<FileInfo>(fileInfo);
            this.Name = this.FileInfo.Select(fileInfo => fileInfo.Name).ToReadOnlyReactivePropertySlim<string>();
            this.FullName = this.FileInfo.Select(fileInfo => fileInfo.FullName).ToReadOnlyReactivePropertySlim<string>();
        }
    }
}
