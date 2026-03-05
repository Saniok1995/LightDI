using System;
using System.Collections.Generic;

namespace Plugins.LightDI
{
    public interface IDependencyContainer
    {
        T Resolve<T>() where T : class;
        T Bind<T>(T dependency) where T : class;
        T Bind<T>(Func<T> fabricMethod) where T : class;
        T Bind<T>() where T : class, new();
        void Bind(object dependency, Type registerType);
        T Unbind<T>() where T : class;
        object Unbind(Type type);
        object Resolve(Type depType);
        public IReadOnlyDictionary<Type, object> Dependencies { get; }
    }
}