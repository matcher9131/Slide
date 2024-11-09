using System;
using System.IO;

namespace Slide.Models.Comparer
{
    public sealed class LastWriteTimeComparer : FileComparerBase
    {
        public override int Compare(FileSystemInfo? x, FileSystemInfo? y)
        {
            // 降順にするために符号を反転する
            return -(x?.LastWriteTime ?? DateTime.MaxValue).CompareTo(y?.LastWriteTime ?? DateTime.MaxValue);
        }

        public static LastWriteTimeComparer Instance { get; } = new LastWriteTimeComparer();
    }
}
