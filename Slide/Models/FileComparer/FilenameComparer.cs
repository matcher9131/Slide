using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Slide.Models.FileComparer
{
    public sealed class FilenameComparer : IComparer<FileSystemInfo?>
    {
        [SuppressUnmanagedCodeSecurity]
        private static class SafeNativeMethods
        {
            [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
            public static extern int StrCmpLogicalW(string x, string y);
        }

        public int Compare(FileSystemInfo? x, FileSystemInfo? y)
        {
            return SafeNativeMethods.StrCmpLogicalW(x?.Name ?? string.Empty, y?.Name ?? string.Empty);
        }
    }
}
