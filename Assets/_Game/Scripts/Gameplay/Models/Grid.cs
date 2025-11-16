namespace BlockSmash.Models
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Grid
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public GridCell[,] Cells { get; private set; }

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            Cells = new GridCell[width, height];
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Cells[x, y] = new GridCell(x, y);
                }
            }
        }

        public bool IsValidPosition(Vector2Int position)
        {
            return position.x >= 0 && position.x < Width && 
                   position.y >= 0 && position.y < Height;
        }

        public bool CanPlaceShape(Shape shape, Vector2Int position)
        {
            foreach (var block in shape.Blocks)
            {
                Vector2Int blockPos = block.Position + position;
                
                if (!IsValidPosition(blockPos))
                    return false;
                
                if (Cells[blockPos.x, blockPos.y].IsOccupied)
                    return false;
            }
            
            return true;
        }

        public void PlaceShape(Shape shape, Vector2Int position)
        {
            foreach (var block in shape.Blocks)
            {
                Vector2Int blockPos = block.Position + position;
                
                if (IsValidPosition(blockPos))
                {
                    Cells[blockPos.x, blockPos.y].State = CellState.Occupied;
                    Cells[blockPos.x, blockPos.y].ColorId = block.ColorId;
                }
            }
        }

        public void ClearCell(int x, int y)
        {
            if (IsValidPosition(new Vector2Int(x, y)))
            {
                Cells[x, y].State = CellState.Empty;
                Cells[x, y].ColorId = -1;
            }
        }

        public List<int> GetFullRows()
        {
            List<int> fullRows = new List<int>();
            
            for (int y = 0; y < Height; y++)
            {
                bool isFull = true;
                for (int x = 0; x < Width; x++)
                {
                    if (Cells[x, y].IsEmpty)
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
            List<int> fullColumns = new List<int>();
            
            for (int x = 0; x < Width; x++)
            {
                bool isFull = true;
                for (int y = 0; y < Height; y++)
                {
                    if (Cells[x, y].IsEmpty)
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
            for (int x = 0; x < Width; x++)
            {
                ClearCell(x, row);
            }
        }

        public void ClearColumn(int column)
        {
            for (int y = 0; y < Height; y++)
            {
                ClearCell(column, y);
            }
        }
    }
}