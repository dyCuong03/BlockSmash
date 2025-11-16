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
        public CellState  state;
        public int        colorId;
        
        public GridCell(Vector2Int position)
        {
            this.position = position;
            this.state    = CellState.Empty;
            this.colorId  = -1;
        }
        
        public GridCell(int x, int y) : this(new Vector2Int(x, y))
        {
        }

        public bool IsEmpty    => this.state == CellState.Empty;
        public bool IsOccupied => this.state == CellState.Occupied;
    }

    public enum CellState
    {
        Empty    = 0,
        Occupied = 1,
    }

    public class Grid
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public GridCell[,] Cells { get; private set; }

        public Grid(int width, int height)
        {
            this.Width  = width;
            this.Height = height;
            this.Cells     = new GridCell[width, height];
            
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
            return position.x >= 0 && position.x < this.Width && 
                   position.y >= 0 && position.y < this.Height;
        }

        public bool CanPlaceShape(ShapeModel shape, Vector2Int position)
        {
            foreach (var blockPos in shape.Blocks.Select(block => block.Position + position))
            {
                if (!this.IsValidPosition(blockPos))
                    return false;
                
                if (this.Cells[blockPos.x, blockPos.y].IsOccupied)
                    return false;
            }

            return true;
        }

        public void PlaceShape(ShapeModel shape, Vector2Int position)
        {
            foreach (var block in shape.Blocks)
            {
                var blockPos = block.Position + position;
                
                if (this.IsValidPosition(blockPos))
                {
                    this.Cells[blockPos.x, blockPos.y].state = CellState.Occupied;
                    this.Cells[blockPos.x, blockPos.y].colorId  = block.ColorId;
                }
            }
        }

        public void ClearCell(int x, int y)
        {
            if (this.IsValidPosition(new (x, y)))
            {
                this.Cells[x, y].state = CellState.Empty;
                this.Cells[x, y].colorId  = -1;
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

        public void ClearRow(int row)
        {
            for (var x = 0; x < this.Width; x++)
            {
                this.ClearCell(x, row);
            }
        }

        public void ClearColumn(int column)
        {
            for (var y = 0; y < this.Height; y++)
            {
                this.ClearCell(column, y);
            }
        }
    }
}