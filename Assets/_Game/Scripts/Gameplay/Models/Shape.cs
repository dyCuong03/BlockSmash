namespace BlockSmash.Models
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Shape
    {
        public int ShapeId { get; private set; }
        public List<Block> Blocks { get; private set; }
        public Vector2Int Pivot { get; private set; }

        public Shape(ShapeData shapeData, int colorId = 0)
        {
            this.ShapeId = shapeData.shapeId;
            this.Blocks  = new List<Block>();
            this.Pivot   = CalculatePivot(shapeData.cells);

            foreach (var cellPosition in shapeData.cells)
            {
                var block = new Block(cellPosition, colorId);
                this.Blocks.Add(block);
            }
        }

        private static Vector2Int CalculatePivot(Vector2Int[] cells)
        {
            if (cells.Length == 0) return Vector2Int.zero;

            var sumX = 0;
            var sumY = 0;

            foreach (var cell in cells)
            {
                sumX += cell.x;
                sumY += cell.y;
            }

            return new Vector2Int(sumX / cells.Length, sumY / cells.Length);
        }

        public void SetPosition(Vector2Int position)
        {
            var offset = position - this.Pivot;
            
            foreach (var block in this.Blocks)
            {
                block.Position += offset;
            }

            this.Pivot = position;
        }
    }
}
