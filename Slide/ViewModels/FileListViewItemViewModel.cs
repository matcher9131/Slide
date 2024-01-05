using Reactive.Bindings;
using Slide.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Reactive.Bindings.Extensions;
using Prism.Mvvm;
using System.IO;

namespace Slide.ViewModels
{
    public class FileListViewItemViewModel : BindableBase, IDisposable
    {
        private readonly FileModel fileModel;

        public ReadOnlyReactivePropertySlim<string> DisplayText { get; }

        public ReactivePropertySlim<bool> IsSelected { get; }

        public FileListViewItemViewModel(FileModel fileModel)
        {
            this.fileModel = fileModel;
            this.DisplayText = this.fileModel.Name.Select(x => x).ToReadOnlyReactivePropertySlim<string>().AddTo(this.disposables);
            this.IsSelected = new ReactivePropertySlim<bool>().AddTo(this.disposables);
        }

        public FileInfo FileInfo => this.fileModel.FileInfo.Value;

        #region IDisposable
        private readonly System.Reactive.Disposables.CompositeDisposable disposables = new();
        public void Dispose() => this.disposables.Dispose();
        #endregion
    }
}
