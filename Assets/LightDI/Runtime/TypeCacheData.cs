using System.Collections.Generic;
using System.Reflection;

namespace Plugins.LightDI
{
    internal class TypeCacheData
    {
        private readonly List<FieldInfo> injectFields;
        public IReadOnlyList<FieldInfo> InjectFields => injectFields;

        public TypeCacheData(List<FieldInfo> injectFields)
        {
            this.injectFields = injectFields;
        }
    }
}