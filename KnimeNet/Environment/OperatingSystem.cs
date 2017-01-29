using System;
using System.IO;
using System.Runtime.InteropServices;
using KnimeNet.Environment.Types.Enums;

namespace KnimeNet.Environment
{
    /// <summary>
    /// Some functionality regarding the host's operating system
    /// </summary>
    public static class OperatingSystem
    {
        /// <summary>
        /// Gets the type of the os.
        /// </summary>
        /// <returns>The hosts <see cref="OsType"/></returns>
        public static OsType GetOsType()
        {
            if (Path.DirectorySeparatorChar == '\\')
                return OsType.Windows;
            if (IsRunningOnMac())
                return OsType.Mac;
            /* Check for Unix after checking Mac because Mac is sometimes reported as Unix */
            return System.Environment.OSVersion.Platform == PlatformID.Unix ? OsType.X11 : OsType.Other;
        }

        /// <summary>
        /// From Managed.Windows.Forms/XplatUI
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        [DllImport("libc")]
        private static extern int uname(IntPtr buffer);

        /// <summary>
        /// Determines whether [is running on mac].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is running on mac]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsRunningOnMac()
        {
            var buffer = IntPtr.Zero;
            try
            {
                buffer = Marshal.AllocHGlobal(8192);
                if (uname(buffer) == 0)
                {
                    var os = Marshal.PtrToStringAnsi(buffer);
                    if (os == "Darwin")
                        return true;
                }
            }
            catch
            {
                // ignored
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                    Marshal.FreeHGlobal(buffer);
            }
            return false;
        }
    }

}
