using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace LightDI.Runtime
{
    public class DependencyContainer : IDependencyContainer
    {
        private readonly Dictionary<Type, object> dependencies;
        private readonly IDependencyContainer root;
        
        public string Tag { get; private set; }
        public IReadOnlyDictionary<Type, object> Dependencies => dependencies;

        public DependencyContainer()
        {
            dependencies = new Dictionary<Type, object>();
            Tag = "Default";
        }

        public DependencyContainer(IDependencyContainer rootContainer, int serviceCount = 10, string tag = "Default")
        {
            dependencies = new Dictionary<Type, object>(serviceCount);
            Tag = tag;

            Assert.IsFalse(rootContainer?.Dependencies == null || rootContainer?.Dependencies.Count == 0,
                "[DependencyLocator] => Create: rootLocator is null or empty");
            
            root = rootContainer;
        }

        public T Resolve<T>() where T : class
        {
            if (dependencies.TryGetValue(typeof(T), out var dependency))
            {
                return dependency as T;
            }

            var resolve = root?.Resolve<T>();
            return resolve;
        }
        
        public object Resolve(Type depType)
        {
            if (dependencies.TryGetValue(depType, out var dependency))
            {
                return dependency;
            }

            var resolve = root?.Resolve(depType);
            return resolve;
        }

        public T Bind<T>(T dependency) where T : class
        {
            dependencies[typeof(T)] = dependency;
            return dependency;
        }
        
        public void Bind(object dependency, Type registerType)
        {
            dependencies[registerType] = dependency;
        }

        public T Bind<T>(Func<T> fabricMethod) where T : class
        {
            var dependency = fabricMethod?.Invoke();
            dependencies[typeof(T)] = dependency;
            return dependency;
        }

        public T Bind<T>() where T : class, new()
        {
            var dependency = new T();
            dependencies[typeof(T)] = dependency;
            return dependency;
        }

        public T Unbind<T>() where T : class
        {
            if (dependencies.TryGetValue(typeof(T), out var dependency))
            {
                dependencies.Remove(typeof(T));
                return dependency as T;
            }

            Debug.LogWarning($"[DependencyLocator] => UnRegister: not found {typeof(T)}");
            return null;
        }
        
        public object Unbind(Type type)
        {
            if (dependencies.Remove(type, out var dependency))
            {
                return dependency;
            }

            Debug.LogWarning($"[DependencyLocator] => UnRegister: not found {type}");
            return null;
        }
    }
}