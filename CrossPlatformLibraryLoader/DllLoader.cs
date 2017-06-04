using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;


namespace CrossLibraryLoader
{
    public class DllLoader : IDisposable
    {
        private readonly Dictionary<string, string> dlls;
        private ConcurrentDictionary<string, IntPtr> loadedLibraries = new ConcurrentDictionary<string, IntPtr>();
        private static ILibraryLoader libraryLoader = new UnversalLibraryLoader();

        public DllLoader(Dictionary<Platforms, Dictionary<string, string>> platformPaths) : this(ResolveDllPlatform(platformPaths))
        {
        }

        public DllLoader(Dictionary<string, string> dlls)
        {
            this.dlls = dlls;
        }

        private void LoadFunctionsForType(Type delegatesHolderType, Object delegatesHolder = null)
        {
            var delegatesToLoad = delegatesHolderType.GetTypeInfo().DeclaredFields
                .Select(fieldInfo => new { fieldInfo = fieldInfo, dllImportFuncAttribute = fieldInfo.FieldType.GetTypeInfo().GetCustomAttributes(typeof(DllImportFunctionAttribute), true)?.Cast<DllImportFunctionAttribute>()?.FirstOrDefault() })
                .Where(field => field.dllImportFuncAttribute != null)
                .ToDictionary(field => field.fieldInfo, field => field.dllImportFuncAttribute);

            var delegatesToLoadByDll = delegatesToLoad.GroupBy(x => x.Value.DllName).ToDictionary(x => x.Key, x => x.ToList());

            foreach (var delegateToLoad in delegatesToLoadByDll)
            {
                var dllPath = delegateToLoad.Key;
                if (dlls.ContainsKey(dllPath) && !string.IsNullOrWhiteSpace(dlls[dllPath]))
                {
                    dllPath = dlls[dllPath];
                    //if (!System.IO.File.Exists(dllPath))
                    //{
                    //    throw new System.IO.FileNotFoundException($"Library file for current platform not found. Check passed paths of library. Checked path: {dllPath}");
                    //}

                }
                LoadFunctionsForDll(delegatesHolder, delegateToLoad.Value, dllPath);
            }
        }
        public void LoadStaticFunctionsForType(Type delegatesHolderType)
        {
            LoadFunctionsForType(delegatesHolderType);
        }

        public void LoadFunctionsForObject(Object delegatesHolder)
        {
            LoadFunctionsForType(delegatesHolder.GetType(), delegatesHolder);
        }

        private void LoadFunctionsForDll(Object delegatesHolder, List<KeyValuePair<FieldInfo, DllImportFunctionAttribute>> fields, string dllPath)
        {
            var libPtr = libraryPtrForDll(dllPath);
            
            foreach(var field in fields)
            {
                LoadFunction(delegatesHolder, field, libPtr);
            }
        }

        private void LoadFunction(Object delegatesHolder, KeyValuePair<FieldInfo, DllImportFunctionAttribute> field, IntPtr libPtr)
        {
            var procName = field.Value.EntryPoint ?? field.Key.Name;
            var procPtr = libraryLoader.GetProcAddress(libPtr, procName);
            
            if(procPtr != IntPtr.Zero)
            {
                var procDelegate = Marshal.GetDelegateForFunctionPointer(procPtr, field.Key.FieldType);
                field.Key.SetValue(delegatesHolder, procDelegate);
            }
        }

        public static Dictionary<string, string> ResolveDllPlatform(Dictionary<Platforms, Dictionary<string, string>> platformDirectories)
        {
            var x86 = RuntimeInformation.OSArchitecture == Architecture.X86;

            Platforms currentPlatform = Platforms.WinX64;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                currentPlatform = x86 ? Platforms.WinX86 : Platforms.WinX64;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                currentPlatform = x86 ? Platforms.LinuxX86 : Platforms.LinuxX64;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                currentPlatform = x86 ? Platforms.MacOsX86 : Platforms.MacOsX64;
            }

            if (!platformDirectories.ContainsKey(currentPlatform))
                throw new Exceptions.ResolveDllsPlatformException(currentPlatform);

            return platformDirectories[currentPlatform];
        }

        private IntPtr libraryPtrForDll(string dllPath)
        {
            return loadedLibraries.GetOrAdd(dllPath, libraryLoader.LoadLibrary(dllPath));
        }

        public void Dispose()
        {
            foreach(var libPtr in loadedLibraries.Values)
            {
                if (libPtr != IntPtr.Zero)
                    libraryLoader.FreeLibrary(libPtr);
            }
        }
    }

    public static class DllLoaderExtensions
    {
        public static void LoadFunctions(this Object delegatesHolder, DllLoader dllLoader) 
        {
            if (dllLoader == null)
                throw new ArgumentNullException("dllLoader", $"DllLoader wasn't initialized");
            dllLoader.LoadFunctionsForObject(delegatesHolder);
        }        
    }

}
