using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public bool[] walls;
    
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

    public void ResetCell()
    {
        isVisited = false;
        endCell = false;
        startCell = false;
        isCurrentCell = false;
        walls = new []{true, true, true, true};
    }
}
