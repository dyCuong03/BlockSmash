namespace Main.Scripts
{
    using BlockSmash.Models;
    using BlockSmash.Pooling;
    using BlockSmash.Services;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    public class MainSceneScope : LifetimeScope
    {
        [SerializeField] private TextAsset  jsonFile;
        [SerializeField] private GameObject levelPrefab;

        private IObjectPoolManager poolManager;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<ObjectPoolManager>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .AsSelf();

            builder.Register<LevelService>(Lifetime.Singleton).AsSelf();
            this.RegisterShapes(builder);
        }

        private void Start()
        {
            this.poolManager = this.Container.Resolve<IObjectPoolManager>();
            this.poolManager.Spawn(this.levelPrefab, Vector3.zero, Quaternion.identity, this.transform);
        }

        private void RegisterShapes(IContainerBuilder builder)
        {
            if (this.jsonFile == null) return;
            var shapeCollection = JsonUtility.FromJson<ShapeCollection>(this.jsonFile.text);

            builder.Register<ShapeService>(Lifetime.Singleton)
                .WithParameter(shapeCollection)
                .AsImplementedInterfaces()
                .AsSelf();
        }
    }
}