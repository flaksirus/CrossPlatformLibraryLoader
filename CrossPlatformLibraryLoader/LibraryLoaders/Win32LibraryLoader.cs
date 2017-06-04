using CrossLibraryLoader.Exceptions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CrossLibraryLoader.LibraryLoaders
{
    internal class Win32LibraryLoader : ILibraryLoader
    {
        private const string kernelLibrary = "kernel32";

        [DllImport(kernelLibrary, EntryPoint = "LoadLibraryW", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr loadLibrary(string libraryName);

        [DllImport(kernelLibrary, EntryPoint = "GetProcAddress", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr getProcAddress(IntPtr hModule, string procName);

        [DllImport(kernelLibrary, EntryPoint = "FreeLibrary", SetLastError = true)]
        private static extern bool freeLibrary(IntPtr hModule);

        public bool FreeLibrary(IntPtr hModule)
        {
            return freeLibrary(hModule);
        }

        public IntPtr GetProcAddress(IntPtr hModule, string procedureName)
        {
            IntPtr hProc = getProcAddress(hModule, procedureName);

            if(hProc == IntPtr.Zero)
                throw new ProcedureLoadException($"Loading procedure \"{procedureName}\" failed: {LastError()}");

            return hProc;
        }

        public IntPtr LoadLibrary(string libraryName)
        {
            IntPtr hModule = loadLibrary(libraryName);

            if (hModule == IntPtr.Zero)
                throw new LibraryLoadException($"Loading library \"{libraryName}\" failed: {LastError()}");
            
            return hModule;
        }

        public string LastError()
        {
            return "Error " + Marshal.GetLastWin32Error();
        }
    }
}
