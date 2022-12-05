using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.CQRS
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class InjectionModeAttribute : Attribute
    {
        public InjectionMode InjectionMode { get; set; }

        public InjectionModeAttribute(InjectionMode injectionMode = InjectionMode.Scoped)
        {
            InjectionMode = injectionMode;
        }
    }
}