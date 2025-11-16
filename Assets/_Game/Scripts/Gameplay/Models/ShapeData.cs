namespace BlockSmash.Models
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class ShapeCollection
    {
        public List<ShapeData> shapes;
    }

    [Serializable]
    public class ShapeData
    {
        public int          shapeId;
        public Vector2Int[] cells;
    }
}