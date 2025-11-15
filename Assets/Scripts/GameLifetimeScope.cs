namespace Main.Scripts
{
    using VContainer;
    using VContainer.Unity;
    using BlockSmash.DI;

    internal sealed class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterBlockSmashCore();
        }
    }
}