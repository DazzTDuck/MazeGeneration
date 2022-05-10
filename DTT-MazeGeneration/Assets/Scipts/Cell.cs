using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cell data
/// </summary>
public class Cell
{
    public int[] wallsIndex = {0, 1, 2, 3};
    
    public bool isVisited;
    public bool isCurrentCell;

    public int x;
    public int y;

    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
        
        ResetCell();
    }

    /// <summary>
    /// Sets the indexes of the wall for each cell
    /// </summary>
    /// <param name="up">Up wallIndex</param>
    /// <param name="left">Left wallIndex</param>
    /// <param name="down">Down wallIndex</param>
    /// <param name="right">Right wallIndex</param>
    public void SetWallIndex(int up, int left, int down, int right)
    {
        wallsIndex = new []{up, left, down, right};
    }

    /// <summary>
    /// Resets the cell data to be used again
    /// </summary>
    public void ResetCell()
    {
        isVisited = false;
        isCurrentCell = false;
    }
}
