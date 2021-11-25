using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public int[] wallsIndex;
    
    public bool isVisited;
    public bool isCurrentCell;
    public bool startCell;
    public bool endCell;

    public int x;
    public int y;

    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
        
        ResetCell();
    }

    public void SetWallIndex(int up, int left, int down, int right)
    {
        wallsIndex = new []{up, left, down, right};
        //Debug.Log($"{x}, {y} = {up}, {left}, {down}, {right}");
    }

    public void ResetCell()
    {
        isVisited = false;
        endCell = false;
        startCell = false;
        isCurrentCell = false;
    }
}
