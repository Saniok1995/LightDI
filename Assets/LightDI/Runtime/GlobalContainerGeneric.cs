namespace Plugins.LightDI
{
    public static class GlobalContainer<T>
        where T : IDependencyContainer, new()
    {
        public static readonly IDependencyContainer Instance;

        static GlobalContainer()
        {
            Instance = new T();
        }
    }
}