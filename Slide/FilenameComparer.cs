using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace Slide
{
    public sealed class FilenameComparer : IComparer<string>
    {
        [SuppressUnmanagedCodeSecurity]
        private static class SafeNativeMethods
        {
            [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
            public static extern int StrCmpLogicalW(string x, string y);
        }

        public int Compare(string? x, string? y)
        {
            return SafeNativeMethods.StrCmpLogicalW(x ?? string.Empty, y ?? string.Empty);
        }
    }
}
