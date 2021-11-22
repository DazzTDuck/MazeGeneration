using System;
using UnityEngine;

public class Grid<TGridObject>
{
    private int width;
    private int height;
    private Vector2 cellSize;
    private Vector2 offset;

    private TGridObject[,] gridArray;

    public Grid(int width, int height, Vector2 cellSize, Vector2 offset, Func<int, int, TGridObject> createCell)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.offset = offset;

        gridArray = new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createCell(x, y);
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

    public TGridObject GetGridObjectValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
            return gridArray[x, y];
        else
            return default(TGridObject);
    }

    public int GetWidth()
    {
        return gridArray.GetLength(0);
    }
    public int GetHeight()
    {
        return gridArray.GetLength(1);
    }
}