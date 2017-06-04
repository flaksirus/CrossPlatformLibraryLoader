using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLibraryLoader.Delegates
{
    internal delegate IntPtr dlopen(string libraryName, int flag);
    internal delegate int dlclose(IntPtr hModule);
    internal delegate IntPtr dlsym(IntPtr hModule, string procName);
    internal delegate IntPtr dlerror();
}
