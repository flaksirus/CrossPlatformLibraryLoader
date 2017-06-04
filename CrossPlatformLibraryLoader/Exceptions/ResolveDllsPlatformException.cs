using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLibraryLoader.Exceptions
{
    public class ResolveDllsPlatformException : Exception
    {
        public ResolveDllsPlatformException() : base() { }

        public ResolveDllsPlatformException(string msg) : base(msg) { }

        public ResolveDllsPlatformException(string msg, Exception innerException) : base(msg, innerException) { }
        public ResolveDllsPlatformException(Platforms platform) : base($"Resolving dlls paths for {platform} failed, check passed platforms paths dictionary") { }

    }
}
