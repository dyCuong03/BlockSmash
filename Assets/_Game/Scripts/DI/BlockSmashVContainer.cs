namespace BlockSmash.DI
{
    using BlockSmash.Managers;
    using BlockSmash.Pooling;
    using VContainer;

    public static class BlockSmashVContainer
    {
        public static void RegisterBlockSmashCore(this IContainerBuilder builder)
        {
            if (builder.Exists(typeof(IObjectPoolManager), true)) return;
            builder.Register<ObjectPoolManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<EntityManager>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
    }
}