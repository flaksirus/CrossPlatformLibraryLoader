using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLibraryLoader
{
    [AttributeUsage(AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
    public class DllImportFunctionAttribute : Attribute
    {
        private string dllName;
        private string entryPoint;

        public DllImportFunctionAttribute(string DllName, string EntryPoint = null)
        {
            this.dllName = DllName;
            this.entryPoint = EntryPoint;
        }

        public string DllName { get => dllName; }
        public string EntryPoint { get => entryPoint; set => entryPoint = value; }
    }
}
