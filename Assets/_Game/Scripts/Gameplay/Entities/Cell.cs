namespace BlockSmash.Entities
{
    using BlockSmash.Models;
    using UnityEngine;

    public class Cell : Entities
    {
        private bool isEmpty = true;
        public bool IsEmpty => this.isEmpty;
        public Vector2Int Position => this.cell.position;
        
        private GridCell cell;

        protected override void OnSpawned()
        {
            base.OnSpawned();
            this.isEmpty = false;
        }

        public void BindData(GridCell data)
        {
            this.cell = data;
        }

        protected override void OnRecycled()
        {
            base.OnRecycled();
        }
    }
}