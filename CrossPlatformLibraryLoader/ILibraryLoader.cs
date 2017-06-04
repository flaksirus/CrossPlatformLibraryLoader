using System;

namespace CrossLibraryLoader
{
    internal interface ILibraryLoader
    {
        /// <summary>
        /// Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.
        /// </summary>
        /// <param name="libraryPath">The name of the module. This can be either a library module (a .dll file) or an executable module (an .exe file). May be relative or full path to module.</param>
        /// <returns>If the function succeeds, the return value is a handle to the module, if not it would be null.</returns>
        IntPtr LoadLibrary(string libraryPath);

        /// <summary>
        /// Retrieves the address of an exported function or variable from the specified dynamic-link library
        /// </summary>
        /// <param name="hModule">A handle to the library module that contains the function or variable.</param>
        /// <param name="procedureName">The function or variable name, or the function's ordinal value. If this parameter is an ordinal value, it must be in the low-order word; the high-order word must be zero.</param>
        /// <returns>If the function succeeds, the return value is the address of the exported function or variable, if not it would be null.</returns>
        IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        /// <summary>
        /// Frees the loaded library  module
        /// </summary>
        /// <param name="hModule">A handle to the loaded library module. </param>
        /// <returns>If the function succeeds, the return value is true, if not it would be false.</returns>
        bool FreeLibrary(IntPtr hModule);

        /// <summary>
        /// Retrieves the calling thread's last-error 
        /// </summary>
        string LastError();
    }
}
