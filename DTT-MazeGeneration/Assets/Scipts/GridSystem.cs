using System;
using UnityEngine;

public class GridSystem
{
    private int width;
    private int height;
    private Vector2 cellSize;
    private Vector2 offset;

    private Cell[,] gridArray;

    /// <summary>
    /// Generates the grid where the maze will be generated
    /// </summary>
    /// <param name="width">Grid width</param>
    /// <param name="height">Grid height</param>
    /// <param name="cellSize">Grid cell size</param>
    /// <param name="offset">Used to get the world position</param>
    public GridSystem(int width, int height, Vector2 cellSize, Vector2 offset)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.offset = offset;

        gridArray = new Cell[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = new Cell(x, y);
            }
        }
    }
    /// <summary>
    /// Gets world position with the given cell coordinates 
    /// </summary>
    /// <param name="x">X coordinates of cell</param>
    /// <param name="y">Y coordinates of cell</param>
    /// <returns></returns>
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * cellSize.x + offset.x, 0, y * cellSize.y + offset.y);
    }
    
    /// <summary>
    /// Returns X & Y coordinates of cell in given world position 
    /// </summary>
    /// <param name="worldPosition">World position of cell you want to get the X & Y coordinates for</param>
    /// <param name="x">returns the X coordinate</param>
    /// <param name="y">returns the Y coordinate</param>
    public void GetGridPositionXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition.x - offset.x) / cellSize.x);
        y = Mathf.FloorToInt((worldPosition.z  - offset.y) / cellSize.y);
    }

    /// <summary>
    /// Returns the cell and it's values that is at the given coordinates
    /// </summary>
    /// <param name="x">X coordinates of cell</param>
    /// <param name="y">Y coordinates of cell</param>
    /// <returns></returns>
    public Cell GetGridObjectValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
            return gridArray[x, y];

        return default(Cell);
    }

    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
}