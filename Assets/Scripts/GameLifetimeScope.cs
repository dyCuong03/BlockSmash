namespace Main.Scripts
{
    using BlockSmash.DI;
    using BlockSmash.Entities;
    using BlockSmash.Managers;
    using BlockSmash.Pooling;
    using BlockSmash.Services;
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
                .WithParameter(this.jsonFile).AsImplementedInterfaces().AsSelf();
            
            builder.Register<GameManager>(
                sp => new GameManager(
                    sp.Resolve<IObjectPoolManager>(),
                    this.levelPrefab,
                    this.transform
                ),
                Lifetime.Singleton
            ).AsSelf().AsImplementedInterfaces();
        }
    }
}