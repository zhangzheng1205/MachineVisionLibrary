using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ComCommunicator
{
    public static class DLLImporter
    {
        /// <summary>
        /// Loads a DLL library.
        /// </summary>
        /// <param name="dllPath">The DLL path.</param>
        /// <returns>The DLL handle</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string dllPath);

        /// <summary>
        /// Gets the address of a procedure in the DLL.
        /// </summary>
        /// <param name="hDll">The DLL handle.</param>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <returns>The procedure pointer</returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hDll, string procedureName);

        /// <summary>
        /// Frees a loaded DLL.
        /// </summary>
        /// <param name="hDll">The DLL handle.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise</returns>
        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hDll);

        /// <summary>
        /// Gets the last error.
        /// </summary>
        /// <returns>The last error code</returns>
        [DllImport("kernel32.dll")]
        public static extern int GetLastError();
    }
}
