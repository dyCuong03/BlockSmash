namespace BlockSmash.Entities
{
    using System.Collections.Generic;
    using BlockSmash.Models;
    using UnityEngine;
    
    public class Block : Entities
    {
        [SerializeField] private SpriteRenderer blockView;
        [SerializeField] private List<Sprite> sprites;

        private BlockModel block;
        private Shape shape;

        private readonly List<Cell> overlappingCells = new();

        public Cell CurrentCell => this.overlappingCells.Count > 0 ? this.GetNearestCell() : null!;

        public bool CanCollect => this.CurrentCell != null;
        
        public bool IsComplete {get; private set;} = false;

        public void BindData(Shape shape, BlockModel data)
        {
            this.block = data;
            this.shape = shape;
            this.blockView.sprite = this.sprites[data.ColorId];
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(this.shape.IsPlaced) return;
            if (other.gameObject.layer != LayerMask.NameToLayer(nameof(Cell))) return;

            var cell = other.GetComponent<Cell>();
            if (!this.overlappingCells.Contains(cell)) this.overlappingCells.Add(cell);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(this.shape.IsPlaced) return;
            if (other.gameObject.layer != LayerMask.NameToLayer(nameof(Cell))) return;

            var cell = other.GetComponent<Cell>();
            this.overlappingCells.Remove(cell);
        }

        private Cell GetNearestCell()
        {
            var blockPos = this.transform.position;
            Cell    nearest  = null!;
            var   minDist  = float.MaxValue;

            foreach (var cell in this.overlappingCells)
            {
                var dist = Vector2.Distance(blockPos, cell.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = cell;
                }
            }

            return nearest;
        }

        protected override void OnRecycled()
        {
            this.IsComplete = false;
            this.overlappingCells.Clear();
        }

        public void OnRemoved()
        {
            this.IsComplete = true;
            this.gameObject.SetActive(false);
        }
    }
}