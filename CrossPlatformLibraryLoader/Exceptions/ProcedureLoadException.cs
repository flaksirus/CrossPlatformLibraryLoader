using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLibraryLoader.Exceptions
{
    public class ProcedureLoadException : Exception
    {
        public ProcedureLoadException() : base() { }

        public ProcedureLoadException(string msg) : base(msg) { }

        public ProcedureLoadException(string msg, Exception innerException) : base(msg, innerException) { }

        public ProcedureLoadException(string procName, string sysError) : base($"Loading procedure \"{procName}\" failed. System error: {sysError}") { }

    }
}
