using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
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
        
        generatingMaze = true;
    }
    
    public void GenerateMaze()
    {
        Debug.Log("generating");

        //to show current cell
        foreach (Cell cell in stack)
        {
            cell.isCurrentCell = cell == stack.First();
        }
        currentCell.isVisited = true;

        Cell nextCell = GetRandomNeighbor(currentCell);
        if(nextCell != null)
            RemoveWalls(currentCell, nextCell);
        
        // Remove wall if it is current cell
        foreach (Cell wall in Walls)
        {
            if (wall.x == currentCell.x && wall.y == currentCell.y)
            {
                wall.isWall = false;
                int index = Walls.IndexOf(wall);
                Walls.RemoveAt(index);
                break;
            }
        }
        
        if(nextCell != null)
        {
            stack.Push(currentCell);
            currentCell = nextCell;
        }
        else if(stack.Count > 0)
            currentCell = stack.Pop();
        
        //check if done 
        generatingMaze = !IsCompleted();
    }
    
    private void RemoveWalls(Cell a, Cell b)
    {
        //assigning coordinates of wall between a and b
        // if not equal, check if it is higher, if true decrease one, else one higher, else just same coordinate.
        
        int x = (a.x != b.x) ? (a.x > b.x ? a.x - 1 : a.x + 1) : a.x;   
        int y = (a.y != b.y) ? (a.y > b.y ? a.y - 1 : a.y + 1) : a.y;

        foreach (Cell wall in Walls)
        {
            if(wall.x == x && wall.y == y)
            {
                wall.isWall = false;
                int index = Walls.IndexOf(wall);
                Walls.RemoveAt(index);
                break;
            }        
        }
        switch (a.x - b.x)
        {
            //Disabling corresponding wall for each cell
            case 1:
            {
                //remove a Top and b Bottom wall
                RemoveWall(a.x - 2, a.y);
                RemoveWall(b.x + 2, b.y);
                break;
            }
            case -1:
                //remove a Bottom and b Top wall
                RemoveWall(a.x + 2, a.y);
                RemoveWall(b.x - 2, b.y);
                break;
        }
        switch (a.y - b.y)
        {
            case 1:
                //remove a Left and b Right wall
                RemoveWall(a.x, a.y - 2);
                RemoveWall(b.x, b.y + 2);
                break;
            case -1:
                //remove a Right and b Left wall
                RemoveWall(a.x, a.y + 2);
                RemoveWall(b.x, b.y - 2);
                break;
        }
    }
    private Cell GetRandomNeighbor(Cell cell)
    {
        List<Cell> neighbors = new List<Cell>();
        
         // Assigning available neighbor cells
            Cell top = (cell.x - 2 > 0) ? Cells.Find(c => c.x == cell.x - 2 && c.y == cell.y) : null;
            Cell right = (cell.y + 1 < gridSystem.GetWidth() - 1) ? Cells.Find(c => c.y == cell.y + 2 && c.x == cell.x) : null;
            Cell bottom = (cell.x + 1 < gridSystem.GetHeight() - 1) ? Cells.Find(c => c.x== cell.x + 2 && c.y == cell.y) : null;
            Cell left = (cell.y - 2 > 0) ? Cells.Find(c => c.y == cell.y - 2 && c.x == cell.y) : null;

            if (top is { isVisited: false })
            {
                neighbors.Add(top);
            }
            if (right is { isVisited: false })
            {
                neighbors.Add(right);
            }
            if (bottom is { isVisited: false })
            {
                neighbors.Add(bottom);
            }
            if (left is { isVisited: false })
            {
                neighbors.Add(left);
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
    
    private void RemoveWall(int x, int y)
    {
        if(x < 0)
            x = 0;
        if(y < 0)
            y = 0;
        
        Debug.Log(x +", " + y);
        
        Cell wall = gridSystem.GetGridObjectValue(x, y);
        wall.isWall = false;
    }
    
    
    public bool IsCompleted()
    {
        return stack.Count == 0;
    }
}
