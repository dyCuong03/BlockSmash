namespace Main.Scripts
{
    using BlockSmash.DI;
    using BlockSmash.Entities;
    using BlockSmash.Managers;
    using BlockSmash.Pooling;
    using BlockSmash.Services;
    using BlockSmash.Signals;
    using BlockSmash.Signals.BlockSmash.Signals;
    using MessagePipe;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    internal sealed class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private TextAsset jsonFile;
        [SerializeField] private Level levelPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterBlockSmashCore();

            builder.Register<ShapeService>(Lifetime.Singleton)
                .WithParameter(this.jsonFile)
                .AsImplementedInterfaces()
                .AsSelf();

            builder.Register<GameManager>(
                sp => new GameManager(
                    sp.Resolve<IObjectPoolManager>(),
                    this.levelPrefab,
                    this.transform,
                    sp.Resolve<ShapeService>()
                ),
                Lifetime.Singleton
            ).AsSelf().AsImplementedInterfaces();

            var options = builder.RegisterMessagePipe();  
            builder.RegisterMessageBroker<ShapePlacedSignal>(options);
            builder.RegisterMessageBroker<GameLoseSignal>(options);
        }
    }
}