namespace BlockSmash.Entities
{
    using UnityEngine;

    public class Block : Entities
    {
        [SerializeField] private SpriteRenderer blockView;

        protected override void OnSpawned()
        {
            base.OnSpawned();
            // Initialize block khi được spawn
            Debug.Log($"Block {this.name} spawned!");
        }

        protected override void OnRecycled()
        {
            base.OnRecycled();
            // Reset block state khi được recycle
            Debug.Log($"Block {this.name} recycled!");
        }
    }
}