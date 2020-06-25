# CrossPlatformLibraryLoader
[![Nuget](https://img.shields.io/nuget/v/CrossPlatformLibraryLoader?color=blue&label=CrossPlatformLibraryLoader)](https://www.nuget.org/packages/CrossPlatformLibraryLoader)

CrossPlatformLibraryLoader is a project which supposed to help load native libraries like .dll .so or .dylib dynamicly in runtime.
It was writen for dotnet core but it can be used with full framework and mono too.

It has been tested on (x64 only):
  - Windows
  - Ubuntu 
  - OsX 
  
# Usage
Add [nuget](https://www.nuget.org/packages/CrossPlatformLibraryLoader) to your project. 
If you came here than you allready have some project where you using native library by `DllImportAttribute` or by `LoadLibrary` and delegeates for functions.
So I will compoare it with classical DllImport. In classical DllImport case you would have something like:
```
public class PInvokeClass
{
    static string LibraryName = "UnmanagedLibrary.dll";
    
    [DllImport(LibraryName, EntryPoint = "foo")]
    public static extern int Foo(IntPtr ptr);
    ...
}
```

To use it on many platforms with platform dependent unmanaged libraries you should convert it to class like this:
```
using CrossLibraryLoader;
public class PInvokeClass
{
    static string LibraryName = "UnmanagedLibrary";
    class delegates
    {
        public delegate int Foo(IntPtr ptr);
    }
    [DllImportFunction(LibraryName, EntryPoint = "foo")]
    public static delegates.Foo Foo;
    ...
    static PInvokeClass()
    {
        var paths = new Dictionary<Platforms, Dictionary<string, string>>();
        paths.Add(Platforms.WinX64, new Dictionary<string, string>());
            paths[Platforms.WinX64].Add( LibraryName, "UnmanagedLibrary.dll");
        paths.Add(Platforms.LinuxX64, new Dictionary<string, string>());
            paths[Platforms.LinuxX64].Add( LibraryName, "UnmanagedLibrary.so");
        paths.Add(Platforms.MacOsX64, new Dictionary<string, string>());
            paths[Platforms.MacOsX64].Add(LibraryName, "UnmanagedLibrary.dylib");
            
        var dllLoader = new DllLoader(paths);
        dllLoader.LoadStaticFunctionsForType(typeof(PInvokeClass));
    }
}
```

Of course it would be good style to move DllLoader initialization routine to dedicated helper class.

# Things to remeber
 - you can set full path to unmanaged library or relative path
 - library to load may load other unmanaged libraries so be sure that paths to them added to environment variables like PATH, LD_LIBRARY_PATH, DYLD_LIBRARY_PATH
