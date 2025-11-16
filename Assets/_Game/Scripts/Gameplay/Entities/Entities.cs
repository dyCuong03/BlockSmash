#nullable enable
namespace BlockSmash.Entities
{
    using UnityEngine;

    public abstract class Entities : MonoBehaviour
    {
        internal void HandleInstantiated()
        {
            this.OnInstantiated();
        }

        internal void HandleSpawned()
        {
            this.OnSpawned();
        }

        internal void HandleRecycled()
        {
            this.OnRecycled();
        }

        internal void HandleCleanedUp()
        {
            this.OnCleanedUp();
        }

        protected virtual void OnInstantiated()
        {
        }

        protected virtual void OnSpawned()
        {
        }

        protected virtual void OnRecycled()
        {
        }

        protected virtual void OnCleanedUp()
        {
        }
    }
}