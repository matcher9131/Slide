using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Slide.Models.Comparer
{
    public sealed class FilenameComparer : FileComparerBase
    {
        [SuppressUnmanagedCodeSecurity]
        private static class SafeNativeMethods
        {
            [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
            public static extern int StrCmpLogicalW(string x, string y);
        }

        public override int Compare(FileSystemInfo? x, FileSystemInfo? y)
        {
            return SafeNativeMethods.StrCmpLogicalW(x?.Name ?? string.Empty, y?.Name ?? string.Empty);
        }

        public static FilenameComparer Instance { get; } = new FilenameComparer();
    }
}
