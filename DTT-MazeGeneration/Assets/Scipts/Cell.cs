using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public bool[] wallsActive;
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

    public void SetWallIndex(int up, int left, int bottom, int right)
    {
        wallsIndex = new []{up, left, bottom, right};
    }

    public void ResetCell()
    {
        isVisited = false;
        endCell = false;
        startCell = false;
        isCurrentCell = false;
        wallsActive = new []{true, true, true, true};
    }
}
