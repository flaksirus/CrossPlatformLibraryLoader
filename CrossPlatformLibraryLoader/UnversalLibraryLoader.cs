using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using CrossLibraryLoader.LibraryLoaders;

namespace CrossLibraryLoader
{
    internal class UnversalLibraryLoader : ILibraryLoader
    {
        private static readonly ILibraryLoader CurrentPlatformLibraryLoader;
        static UnversalLibraryLoader()
        {            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                CurrentPlatformLibraryLoader = new Win32LibraryLoader();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                CurrentPlatformLibraryLoader = new LinuxLibraryLoader();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                CurrentPlatformLibraryLoader = new MacLibraryLoader();
            }
        }
        public bool FreeLibrary(IntPtr hModule) => CurrentPlatformLibraryLoader.FreeLibrary(hModule);

        public IntPtr GetProcAddress(IntPtr hModule, string procedureName) => CurrentPlatformLibraryLoader.GetProcAddress(hModule, procedureName);

        public string LastError() => CurrentPlatformLibraryLoader.LastError();

        public IntPtr LoadLibrary(string libraryPath) => CurrentPlatformLibraryLoader.LoadLibrary(libraryPath);

        private static void UpdateEnvironmentVariables()
        {
            var path = Environment.GetEnvironmentVariable("PATH");
        }
    }
}
