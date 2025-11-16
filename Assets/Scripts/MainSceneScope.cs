namespace Main.Scripts
{
    using BlockSmash.Models;
    using BlockSmash.Services;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    public class MainSceneScope : LifetimeScope
    {
        [SerializeField] private TextAsset jsonFile;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<LevelService>(Lifetime.Singleton).AsSelf();
            this.RegisterShapes(builder);
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
