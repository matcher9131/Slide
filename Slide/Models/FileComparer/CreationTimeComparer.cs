using System;
using System.Collections.Generic;
using System.IO;

namespace Slide.Models.FileComparer
{
    public sealed class CreationTimeComparer : IComparer<FileSystemInfo?>
    {
        public int Compare(FileSystemInfo? x, FileSystemInfo? y)
        {
            return (x?.CreationTime ?? DateTime.MaxValue).CompareTo(y?.CreationTime ?? DateTime.MaxValue);
        }
    }
}
