﻿using System;
using System.IO;

namespace Slide.Models.Comparer
{
    public sealed class CreationTimeComparer : FileComparerBase
    {
        public override int Compare(FileSystemInfo? x, FileSystemInfo? y)
        {
            return (x?.CreationTime ?? DateTime.MaxValue).CompareTo(y?.CreationTime ?? DateTime.MaxValue);
        }

        public static CreationTimeComparer Instance { get; } = new CreationTimeComparer();
    }
}