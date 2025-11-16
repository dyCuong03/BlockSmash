namespace BlockSmash.Entities
{
    using UnityEngine;

    public class Cell : Entities
    {
        private bool isEmpty = true;
        
        public bool IsEmpty => this.isEmpty;
        
        public Vector2Int Position { get; private set; }

        protected override void OnSpawned()
        {
            base.OnSpawned();
            this.isEmpty = false;
        }
    }
}