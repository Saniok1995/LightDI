using System;

namespace Plugins.LightDI
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
        
    }
}