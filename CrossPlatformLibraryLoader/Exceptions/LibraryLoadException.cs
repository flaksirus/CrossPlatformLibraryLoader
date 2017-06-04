using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLibraryLoader.Exceptions
{
    public class LibraryLoadException : Exception
    {
        public LibraryLoadException() : base() { }

        public LibraryLoadException(string msg) : base(msg) { }

        public LibraryLoadException(string msg, Exception innerException) : base(msg, innerException) { }
        public LibraryLoadException(string library, string sysEror) : base($"Loading library \"{library}\" failed. System error: {sysEror}") { }

    }
}
