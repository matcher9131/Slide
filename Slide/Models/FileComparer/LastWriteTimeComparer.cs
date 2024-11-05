using System;
using System.Collections.Generic;
using System.IO;

namespace Slide.Models.FileComparer
{
    public sealed class LastWriteTimeComparer : IComparer<FileSystemInfo?>
    {
        public int Compare(FileSystemInfo? x, FileSystemInfo? y)
        {
            return (x?.LastWriteTime ?? DateTime.MaxValue).CompareTo(y?.LastWriteTime ?? DateTime.MaxValue);
        }
    }
}
