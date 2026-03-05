namespace LightDI.Runtime
{
    public static class GlobalContainer
    {
        public static readonly IDependencyContainer Instance;

        static GlobalContainer()
        {
            Instance = new DependencyContainer();
        }
    }
}