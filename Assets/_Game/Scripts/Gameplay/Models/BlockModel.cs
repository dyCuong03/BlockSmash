namespace BlockSmash.Models
{
    using UnityEngine;

    public class BlockModel
    {
        public Vector2Int Position { get; set; }
        public int        ColorId  { get; set; }

        public BlockModel(Vector2Int position, int colorId)
        {
            this.Position = position;
            this.ColorId  = colorId;
        }

        public BlockModel(int x, int y, int colorId) : this(new Vector2Int(x, y), colorId)
        {
        }
    }
}

