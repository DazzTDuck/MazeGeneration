using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using Random = System.Random;

public class MazeGeneration
{
    public GridSystem gridSystem {get; set;}
    public bool generatingMaze = false;

    private Stack<Cell> stack = new Stack<Cell>();
    
    private Cell currentCell; 

    public MazeGeneration(GridSystem gridSystem, bool generatingMaze)
    {
        this.gridSystem = gridSystem;
        this.generatingMaze = generatingMaze;
    }

    public void SetupGeneration()
    {
        ResetMaze();
        Debug.Log("Setup");
        
    }
    
    public void GenerateMaze()
    {
        Debug.Log("generating");
        
    }

    public void BreakWallBetweenCells(Cell a, Cell b)
    {
        //break wall between 2 cells given    
    }
    
    public Cell GetRandomNeighbor(Cell cell)
    {
        List<Cell> neighbors = new List<Cell>();
        
        //get neighbor that is up, down, left and right of the given cell
        
        return null;
    } 

    public void ResetMaze()
    {
        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int y = 0; y < gridSystem.GetHeight(); y++)
            {
                var cell = gridSystem.GetGridObjectValue(x, y);
                cell.ResetCell();
            }
        }
    }
    
}
