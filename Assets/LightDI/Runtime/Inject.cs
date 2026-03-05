using System;

namespace LightDI.Runtime
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
        
    }
}