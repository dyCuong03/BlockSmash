namespace BlockSmash.Models
{
    using System;
    using UnityEngine;

    [Serializable]
    public struct GridCell
    {
        public Vector2Int Position;
        public CellState State;
        public int ColorId;
        
        public GridCell(Vector2Int position)
        {
            Position = position;
            State = CellState.Empty;
            ColorId = -1;
        }
        
        public GridCell(int x, int y) : this(new Vector2Int(x, y))
        {
        }

        public bool IsEmpty => State == CellState.Empty;
        public bool IsOccupied => State == CellState.Occupied;
    }

    public enum CellState
    {
        Empty = 0,
        Occupied = 1,
        Locked = 2
    }
}
