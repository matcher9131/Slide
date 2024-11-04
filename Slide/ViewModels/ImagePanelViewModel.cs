using Prism.Events;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Slide.Behavior;
using Slide.Models;
using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Media.Imaging;

namespace Slide.ViewModels
{
    public class ImagePanelViewModel : BindableBase, IDisposable
    {
        private readonly IEventAggregator eventAggregator;
        private readonly SelectedItemModel selectedItemModel;

        public ReadOnlyReactiveProperty<BitmapSource?> Source { get; }

        public ReactiveCommand<ClickPosition> ClickCommand { get; }

        public ImagePanelViewModel(IEventAggregator eventAggregator, SelectedItemModel selectedItemModel)
        {
            this.eventAggregator = eventAggregator;
            this.selectedItemModel = selectedItemModel;
            this.Source = this.selectedItemModel.SelectedFile.Select(selectedFile =>
            {
                if (selectedFile != null)
                {
                    try
                    {
                        return LoadImage(selectedFile.FullName);
                    }
                    catch (IOException)
                    {
                        return null;
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        System.Diagnostics.Debug.WriteLine(ex);
#endif
                        try
                        {
                            return (BitmapSource?)LoadImage2(selectedFile.FullName);
                        }
                        catch (Exception innerEx)
                        {
#if DEBUG
                            System.Diagnostics.Debug.WriteLine(innerEx);
#endif
                            return null;
                        }
                    }
                }
                else
                {
                    return null;
                }
            }).ToReadOnlyReactiveProperty().AddTo(this.disposables);
            this.ClickCommand = new ReactiveCommand<ClickPosition>().WithSubscribe(this.OnClick).AddTo(this.disposables);
        }

        private static BitmapFrame? LoadImage(string filepath)
        {
            using var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            var decoder = BitmapDecoder.Create(fs, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            return decoder.Frames[0];
        }

        private static BitmapImage LoadImage2(string filepath)
        {
            System.Drawing.Bitmap bitmap = new(filepath);
            using var stream = new MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            stream.Position = 0;
            BitmapImage bitmapImage = new();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.CreateOptions = BitmapCreateOptions.None;
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return bitmapImage;
        }

        private void OnClick(ClickPosition clickPosition)
        {
            this.eventAggregator.GetEvent<ClickPositionEvent>().Publish(clickPosition);
        }

        #region IDisposable
        private readonly System.Reactive.Disposables.CompositeDisposable disposables = new();
        public void Dispose() => this.disposables.Dispose();
        #endregion
    }
}
