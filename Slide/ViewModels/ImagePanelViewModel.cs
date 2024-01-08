using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Slide.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Slide.ViewModels
{
    public class ImagePanelViewModel : BindableBase, IDisposable
    {
        private readonly SelectedItemModel selectedItemModel;

        public ReadOnlyReactiveProperty<BitmapSource?> Source { get; }

        public ImagePanelViewModel(SelectedItemModel selectedItemModel)
        {
            this.selectedItemModel = selectedItemModel;
            this.Source = this.selectedItemModel.SelectedFile.Select(selectedFile =>
            {
                if (selectedFile != null)
                {
                    try
                    {
                        using var fs = new FileStream(selectedFile.FullName, FileMode.Open, FileAccess.Read);
                        var decoder = BitmapDecoder.Create(fs, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                        return (BitmapSource?)decoder.Frames[0];
                    }
                    catch (IOException)
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }).ToReadOnlyReactiveProperty().AddTo(this.disposables);
        }

        #region IDisposable
        private readonly System.Reactive.Disposables.CompositeDisposable disposables = new();
        public void Dispose() => this.disposables.Dispose();
        #endregion
    }
}
