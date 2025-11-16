namespace BlockSmash.Models
{
    using UnityEngine;

    public class Block
    {
        public Vector2Int Position { get; set; }
        public int ColorId { get; set; }
        public bool IsActive { get; set; }

        public Block(Vector2Int position, int colorId)
        {
            this.Position = position;
            this.ColorId  = colorId;
            this.IsActive    = true;
        }

        public Block(int x, int y, int colorId) : this(new Vector2Int(x, y), colorId)
        {
        }
    }
}
