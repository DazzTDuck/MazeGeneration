using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class MazeGeneration
{
    public GridSystem gridSystem {get; set;}
    public bool generatingMaze = false;
    public List<Cell> Cells { get; set;}
    public List<Cell> Walls { get; set;}
    
    private List<Cell> stack = new List<Cell>();    
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

        //select random starting cell
        SetStartAndEndCell(true, false);
        //select random end cell
        SetStartAndEndCell(false, true);
        
        //implement algorithm
        Cells = new List<Cell>();
        Walls = new List<Cell>();

        //adding cells
        for (int x = 1; x < gridSystem.GetWidth(); x+= 2)
        {
            for (int y = 1; y < gridSystem.GetHeight(); y+= 2)
            {
                Cells.Add(gridSystem.GetGridObjectValue(x, y));   
            }    
        }
        //adding walls
        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int y = 0; y < gridSystem.GetHeight(); y++)
            {
                Walls.Add(gridSystem.GetGridObjectValue(x, y));     
            }    
        }
        
        //set walls
        for (int i = 0; i < Walls.Count; i++)
        {
            if(Walls[i].endCell || Walls[i].startCell)
            {
                Walls.RemoveAt(i);
            }
            Walls[i].isWall = true;
        }

        generatingMaze = false;
    }
    
    public void GenerateMaze(int latency)
    {
        do
        {
           //check if done 
           generatingMaze = !IsCompleted();    
        }
        while (generatingMaze);
        
    }
    
    private Cell GetRandomNeighbor(Cell cell)
    {
        List<Cell> neighbors = new List<Cell>();
        
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                int checkX = cell.x + x;
                int checkY = cell.y + y;

                if (checkX >= 0 && checkX < gridSystem.GetWidth() && checkY >= 0 && checkY < gridSystem.GetHeight())
                {
                    neighbors.Add(gridSystem.GetGridObjectValue(checkX, checkY));
                }
            }
        }
        
        if (neighbors.Count > 0)
        {
            int index = Random.Range(0, neighbors.Count);
            return neighbors[index];
        }
        return null;  
    }

    public void SetStartAndEndCell(bool startCell, bool endCell)
    {
        RandomizeBorderCell(out Cell randomCell);
        randomCell.startCell = startCell;
        if(startCell)
            currentCell = randomCell;
        
        if(endCell)
        {
            while (randomCell == currentCell)
            {
                RandomizeBorderCell(out randomCell);        
            }
            randomCell.endCell = true;
        }
        randomCell.isWall = false;
    }
    
    public void RandomizeBorderCell(out Cell cell)
    {
        if (Random01(1, 0) == 1)
        {
            int randomY = Random.Range(1, gridSystem.GetHeight() - 1);
            cell = gridSystem.GetGridObjectValue(Random01(0, gridSystem.GetWidth() - 1), randomY);
        }
        else
        {
            int randomX = Random.Range(1, gridSystem.GetWidth() - 1);
            cell = gridSystem.GetGridObjectValue(randomX, Random01(0, gridSystem.GetHeight() - 1));
        }
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

    public static int Random01(int trueValue, int falseValue)
    {
        int randomSide = Random.Range(0, 2);
        bool hit = randomSide == 1;

        return hit ? trueValue : falseValue;
    }
    
    public bool IsCompleted()
    {
        return stack.Count == 0;
    }
}
