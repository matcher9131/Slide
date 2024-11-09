using System.Collections.Generic;
using System.IO;

namespace Slide.Models.Comparer
{
    public abstract class FileComparerBase : IComparer<FileSystemInfo?>
    {
        public abstract int Compare(FileSystemInfo? x, FileSystemInfo? y);
    }
}
