using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CrossLibraryLoader.Delegates;

namespace CrossLibraryLoader.LibraryLoaders
{
    internal class LinuxLibraryLoader : NixLoader
    {
        private const string kernelLibrary = "libdl.so.2";

        [DllImport(kernelLibrary)]
        private static extern IntPtr dlopen(string libraryName, int flag);

        [DllImport(kernelLibrary)]
        private static extern int dlclose(IntPtr hModule);

        [DllImport(kernelLibrary)]
        private static extern IntPtr dlsym(IntPtr hModule, string procName);

        [DllImport(kernelLibrary)]
        private static extern IntPtr dlerror();

        public LinuxLibraryLoader() : base(new dlopen(dlopen), new dlclose(dlclose), new dlsym(dlsym), new dlerror(dlerror))
        {
        }

    }
}
