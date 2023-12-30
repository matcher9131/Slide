using System.Windows.Media.Imaging;

namespace Slide
{
    public record BitmapImageModel(
        string Filepath,
        BitmapImage Thumbnail
    );
}
