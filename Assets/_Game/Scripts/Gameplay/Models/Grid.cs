namespace BlockSmash.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    [Serializable]
    public struct GridCell
    {
        public Vector2Int position;
        public int        colorId;

        public GridCell(Vector2Int position)
        {
            this.position = position;
            this.colorId  = -1;
        }

        public GridCell(int x, int y) : this(new Vector2Int(x, y))
        {
        }

        public bool IsEmpty    => this.colorId == -1;
        public bool IsOccupied => this.colorId != -1;
    }

    public class Grid
    {
        public int         Width  { get; private set; }
        public int         Height { get; private set; }
        public GridCell[,] Cells  { get; private set; }

        public Grid(int width, int height)
        {
            this.Width  = width;
            this.Height = height;
            this.Cells  = new GridCell[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    this.Cells[x, y] = new GridCell(x, y);
                }
            }
        }

        public bool IsValidPosition(Vector2Int position)
        {
            return position.x >= 0 && position.x < this.Width && position.y >= 0 && position.y < this.Height;
        }
        
        public bool TryPlaceShape(List<Vector2Int> positions, int colorId = 0)
        {
            foreach (var pos in positions)
            {
                if (!this.IsValidPosition(pos))
                    return false;

                if (this.Cells[pos.x, pos.y].IsOccupied)
                    return false;
            }

            foreach (var pos in positions)
            {
                var cell = this.Cells[pos.x, pos.y];
                cell.colorId          = colorId;
                this.Cells[pos.x, pos.y] = cell;
            }

            return true;
        }

        public void PlaceShape(List<Vector2Int> positions, int colorId = 0)
        {
            foreach (var position in positions)
            {
                if (!this.IsValidPosition(position))
                    throw new InvalidOperationException($"Invalid position {position} for placing shape.");

                var cell = this.Cells[position.x, position.y];
                cell.colorId                  = colorId;
                this.Cells[position.x, position.y] = cell;
            }
        }

        public List<int> GetFullRows()
        {
            var fullRows = new List<int>();

            for (var y = 0; y < this.Height; y++)
            {
                var isFull = true;
                for (var x = 0; x < this.Width; x++)
                {
                    if (this.Cells[x, y].IsEmpty)
                    {
                        isFull = false;
                        break;
                    }
                }

                if (isFull)
                    fullRows.Add(y);
            }

            return fullRows;
        }

        public List<int> GetFullColumns()
        {
            var fullColumns = new List<int>();

            for (var x = 0; x < this.Width; x++)
            {
                var isFull = true;
                for (var y = 0; y < this.Height; y++)
                {
                    if (this.Cells[x, y].IsEmpty)
                    {
                        isFull = false;
                        break;
                    }
                }

                if (isFull)
                    fullColumns.Add(x);
            }

            return fullColumns;
        }
    }
}