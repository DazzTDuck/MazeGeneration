using System;
using UnityEngine;

public class GridSystem
{
    private int width;
    private int height;
    private Vector2 cellSize;
    private Vector2 offset;

    private Cell[,] gridArray;

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
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * cellSize.x + offset.x, 0, y * cellSize.y + offset.y);
    }
    
    public void GetGridPositionXY(Vector3 worldPosition, out int x, out int y)
    {
        //sets world position to grid position
        x = Mathf.FloorToInt((worldPosition.x - offset.x) / cellSize.x);
        y = Mathf.FloorToInt((worldPosition.z  - offset.y) / cellSize.y);
    }

    public Cell GetGridObjectValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
            return gridArray[x, y];
        else
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