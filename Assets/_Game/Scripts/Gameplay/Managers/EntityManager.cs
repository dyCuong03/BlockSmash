#nullable enable
namespace BlockSmash.Managers
{
    using BlockSmash.Entities;
    using BlockSmash.Pooling;
    using UnityEngine;
    using VContainer.Unity;

    public sealed class EntityManager : IInitializable
    {
        private readonly IObjectPoolManager objectPoolManager;

        public EntityManager(IObjectPoolManager objectPoolManager)
        {
            this.objectPoolManager = objectPoolManager;
        }
        
        public void Initialize()
        {
            this.RegisterPoolEvents();
        }

        private void RegisterPoolEvents()
        {
            this.objectPoolManager.Instantiated += OnInstantiatedEvent;
            this.objectPoolManager.Spawned      += OnSpawnedEvent;
            this.objectPoolManager.Recycled     += OnRecycledEvent;
            this.objectPoolManager.CleanedUp    += OnCleanedUpEvent;
        }

        private static void OnInstantiatedEvent(GameObject instance)
        {
            var entity = instance.GetComponent<Entities>();
            if (entity != null)
            {
                entity.HandleInstantiated();
            }
        }

        private static void OnSpawnedEvent(GameObject instance)
        {
            var entity = instance.GetComponent<Entities>();
            if (entity != null)
            {
                entity.HandleSpawned();
            }
        }

        private static void OnRecycledEvent(GameObject instance)
        {
            var entity = instance.GetComponent<Entities>();
            if (entity != null)
            {
                entity.HandleRecycled();
            }
        }

        private static void OnCleanedUpEvent(GameObject instance)
        {
            var entity = instance.GetComponent<Entities>();
            if (entity != null)
            {
                entity.HandleCleanedUp();
            }
        }
    }
}