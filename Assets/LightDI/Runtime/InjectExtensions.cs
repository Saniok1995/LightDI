using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LightDI.Runtime
{
    public static class InjectExtensions
    {
        private static readonly Dictionary<Type, TypeCacheData> typeCache = new(40);
        
        private static readonly HashSet<Type> skipTypes = new()
        {
            typeof(MonoBehaviour),
            typeof(Component),
            typeof(object),
        };

        public static void Inject(this object obj, IDependencyContainer container)
        {
            var type = obj.GetType();

            if (!typeCache.TryGetValue(type, out var typeCacheData))
            {
                typeCacheData = CreateCacheData(type);
                typeCache[type] = typeCacheData;
            }
                
            foreach (var filed in typeCacheData.InjectFields)
            {
                var dep = container.Resolve(filed.FieldType);
                filed.SetValue(obj, dep);
            }
        }

        public static void Inject(this object obj)
        {
            obj.Inject(GlobalContainer.Instance);
        }
        
        private static TypeCacheData CreateCacheData(Type type)
        {
            var injectFields = new List<FieldInfo>();

            while (type != null)
            {
                if (skipTypes.Contains(type))
                {
                    break;
                }
                
                var fields = type.GetFields(BindingFlags.Instance |
                                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                
                for (var i = 0; i < fields.Length; i++)
                {
                    var filed = fields[i];
                    
                    if (filed.IsDefined(typeof(InjectAttribute), inherit: false))
                    {
                        injectFields.Add(filed);
                    }
                }
                
                type = type.BaseType;
            }

            return new TypeCacheData(injectFields);
        }
        
        public static void ClearCache()
        {
            typeCache.Clear();
        }
    }
}