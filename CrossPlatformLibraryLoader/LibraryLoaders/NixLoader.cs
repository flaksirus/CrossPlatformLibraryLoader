using CrossLibraryLoader.Delegates;
using CrossLibraryLoader.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CrossLibraryLoader.LibraryLoaders
{
    internal class NixLoader : ILibraryLoader
    {
        internal readonly dlopen open;
        internal readonly dlclose close;
        internal readonly dlsym sym;
        internal readonly dlerror error;

        [Flags]
        internal enum DlsFlags : int
        {
            RTLD_LAZY       = 0x00001, //Lazy function call binding.
            RTLD_NOW        = 0x00002, //Immediate function call binding.
            RTLD_BINDING_MASK   = 0x3, //Mask of binding time value. 
            RTLD_NOLOAD     = 0x00004, //Do not load the object. 
            RTLD_DEEPBIND   = 0x00008, //Use deep binding
            RTLD_GLOBAL     = 0x00100, //If the following bit is set in the MODE argument to `dlopen', the symbols of the loaded object and its dependencies are made visible as if the object were linked directly into the program.
            RTLD_LOCAL      = 0,       //Unix98 demands the following flag which is the inverse to RTLD_GLOBAL. The implementation does this by default and so we can define the value to zero.
            RTLD_NODELETE   = 0x01000 //Do not delete object when closed.
        }
        public NixLoader(dlopen open, dlclose close, dlsym sym, dlerror error)
        {
            this.open = open;
            this.close = close;
            this.sym = sym;
            this.error = error;
        }

        public IntPtr LoadLibrary(string libraryPath)
        {
            IntPtr hModule = open(libraryPath, (int)DlsFlags.RTLD_LAZY);

            if (hModule == IntPtr.Zero)
                throw new LibraryLoadException(libraryPath, LastError());

            return hModule;
        }

        public IntPtr GetProcAddress(IntPtr hModule, string procedureName)
        {
            IntPtr hProc = sym(hModule, procedureName);

            if (hProc == IntPtr.Zero)
                throw new ProcedureLoadException(procedureName, LastError());

            return hProc;
        }

        public bool FreeLibrary(IntPtr hModule)
        {
            int exitCode = close(hModule);

            return exitCode == 0;
        }

        public string LastError()
        {
            return Marshal.PtrToStringAnsi(error());
        }
    }
}
