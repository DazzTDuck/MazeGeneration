using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public bool isVisited;
    public bool isWall;
    public bool startCell;
    public bool endCell;

    public int x;
    public int y;

    public Cell(int x, int y, bool isVisited = false, bool isWall = true, bool startCell = false, bool endCell = false)
    {
        this.isVisited = isVisited;
        this.isWall = isWall;
        this.endCell = endCell;
        this.startCell = startCell;
        this.x = x;
        this.y = y;
    }

    public Vector2 GetID()
    {
        return new Vector2(x, y);
    }

    public void ResetCell()
    {
        isVisited = false;
        isWall = false;
        endCell = false;
        startCell = false;
    }
}
