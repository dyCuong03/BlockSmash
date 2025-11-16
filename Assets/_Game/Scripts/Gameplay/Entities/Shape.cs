namespace BlockSmash.Entities
{
    using BlockSmash.Extensions;
    using BlockSmash.Implements;
    using BlockSmash.Pooling;
    using UnityEngine;
    using VContainer;

    public class Shape : Entities
    {
        [SerializeField] private GameObject blockPrefab;
        [SerializeField] private GameObject root;

        private IObjectPoolManager objectPoolManager;

        protected override void OnInstantiated()
        {
            this.objectPoolManager = this.GetCurrentContainer().Resolve<IObjectPoolManager>();
        }

        protected override void OnSpawned()
        {
            base.OnSpawned();
        }

        protected override void OnRecycled()
        {
            base.OnRecycled();
        }
    }
}