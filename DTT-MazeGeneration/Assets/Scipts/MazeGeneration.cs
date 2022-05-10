using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Random = UnityEngine.Random;

public class MazeGeneration
{
    public GridSystem gridSystem {get; set;}
    public bool generatingMaze = false;

    private List<GameObject> walls = new List<GameObject>();
    private List<Cell> cells = new List<Cell>();

    private Stack<Cell> stack = new Stack<Cell>();
    
    private Cell currentCell; 

    public MazeGeneration(GridSystem gridSystem, bool generatingMaze)
    {
        this.gridSystem = gridSystem;
        this.generatingMaze = generatingMaze;
    }
    public void SetupGeneration(List<GameObject> walls)
    {
        Debug.Log("Setup");
        this.walls = walls;

        //resetting all cells so it can generate a new maze
        ResetMaze();

        //making sure every cell is referenced in the list
        SetAllCellsInList();

        //set walls in every cell
        SetWallIndexesToCells();

        //Begin generation on (1,1)
        currentCell = cells.First();
        generatingMaze = true;
        
        //start generation
        GenerateMaze();
    }

    /// <summary>
    ///  Tells all the cells what the index is for their corresponding walls on each side
    /// </summary>
    public void SetWallIndexesToCells()
    {
        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int y = 0; y < gridSystem.GetHeight(); y++)
            {
                Cell cell = gridSystem.GetGridObjectValue(x, y);
                
                //Indexes
                int upWallIndex;
                int downWallIndex;
                int rightWallIndex;
                int currentCellIndex = cells.IndexOf(cell);

                //calculate up index
                if(y == 0)
                    upWallIndex = currentCellIndex * 2 + x;
                else if(x == gridSystem.GetWidth() - 1)
                    upWallIndex = currentCellIndex * 2 + x + y + 1;
                else
                    upWallIndex = currentCellIndex * 2 + x + 1;

                //calculate left index
                int leftWallIndex = upWallIndex + 1;
                
                //calculate down index
                if(y == 1 && x == gridSystem.GetWidth() - 1)
                    downWallIndex = upWallIndex - 4;
                else if(x == gridSystem.GetWidth() - 1 || y == 1)
                    downWallIndex = upWallIndex - 3;
                else if(y < 2)
                    downWallIndex = upWallIndex + 2 + y;
                else
                    downWallIndex = upWallIndex - 2;

                //calculate right index
                if(x == gridSystem.GetWidth() - 1)
                    rightWallIndex = upWallIndex + 2;
                else if (x == gridSystem.GetWidth() - 2)
                    rightWallIndex = upWallIndex + (gridSystem.GetHeight() * 2) + 2 + y;
                else
                    rightWallIndex = upWallIndex + (gridSystem.GetHeight() * 2) + 2;

                cell.SetWallIndex(upWallIndex,leftWallIndex,downWallIndex,rightWallIndex);
            }    
        }
    }

    /// <summary>
    /// Adds all the cells of the grid in a list
    /// </summary>
    private void SetAllCellsInList()
    {
        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int y = 0; y < gridSystem.GetHeight(); y++)
            {
                Cell cell = gridSystem.GetGridObjectValue(x, y);
                cells.Add(cell);
            }    
        }
    }

    /// <summary>
    /// The A* algorithm that generates the maze
    /// </summary>
    public void GenerateMaze()
    {
        Debug.Log("generating");

        foreach (Cell cell in stack)
        {
            cell.isCurrentCell = cell == stack.First();
        }
        currentCell.isVisited = true;

        Cell nextCell = GetRandomNeighbor(currentCell);
        if(nextCell != null)
            RemoveWallBetweenCells(currentCell, nextCell);

        if(nextCell != null)
        {
            stack.Push(currentCell);
            currentCell = nextCell;
        }
        else if(stack.Count > 0)
            currentCell = stack.Pop();
        
        //check if done 
        generatingMaze = !IsCompleted();
        
        if(IsCompleted())
        {
            //make opening for start and end point
            MakeStartAndEndPoint();
            Debug.Log("Done"); 
        }
    }

    /// <summary>
    ///  Removes the wall between 2 cells
    /// </summary>
    /// <param name="a">First Cell</param>
    /// <param name="b">Second Cell</param>
    public void RemoveWallBetweenCells(Cell a, Cell b)
    {
        //break wall between 2 cells given
        foreach (int wallA in a.wallsIndex)
        {
            foreach (int wallB in b.wallsIndex)
            {
                if(wallA == wallB)
                {
                    walls[wallA].SetActive(false);
                    return;
                }
            }
        }
        Debug.LogWarning($"cell {a.x},{a.y} and cell {b.x},{b.y} does not have a shared wall");
    }
    
    /// <summary>
    /// This randomized the start position for the maze and makes an exit on the other side of the start point
    /// </summary>
    public void MakeStartAndEndPoint()
    {
        Cell endCell;
        
        int x = 0;
        int y = 0;
        
        if(Random.value > 0.5f) //50% chance  
            x = Random.Range(1, gridSystem.GetWidth() - 1); //bottom row
        else  
            y = Random.Range(1, gridSystem.GetHeight() - 1); //left row
       
        Cell startCell = gridSystem.GetGridObjectValue(x, y);
        
        //open cell based of value
        if(x >= 1 && y == 0)
        {
            //on bottom row remove bottom wall
            walls[startCell.wallsIndex[2]].SetActive(false);
            
            //get random endCell on top row and remove up wall
            endCell = gridSystem.GetGridObjectValue(Random.Range(1, gridSystem.GetWidth() - 1), gridSystem.GetHeight() - 1);
            walls[endCell.wallsIndex[0]].SetActive(false);
        }
        else if(y >= 1 && x == 0)
        {
            //left row
            walls[startCell.wallsIndex[1]].SetActive(false);
            
            //open end cell op right row
            endCell = gridSystem.GetGridObjectValue(gridSystem.GetWidth() - 1, Random.Range(1, gridSystem.GetHeight() - 1));
            walls[endCell.wallsIndex[3]].SetActive(false);
        }
        
        //Wall indexes directions 
        //0 = Up
        //1 = Left
        //2 = Down
        //3 = Right
    }
    
    /// <summary>
    /// Returns a random cell that is next to the given cell
    /// </summary>
    /// <param name="cell">Cell to get a random neighbor from</param>
    /// <returns></returns>
    public Cell GetRandomNeighbor(Cell cell)
    {
        List<Cell> neighbors = new List<Cell>();

        //manipulate the x and y coords to get all cells next to that cell
        Cell upCell = gridSystem.GetGridObjectValue(cell.x, cell.y + 1);
        Cell leftCell = gridSystem.GetGridObjectValue(cell.x - 1, cell.y);
        Cell downCell = gridSystem.GetGridObjectValue(cell.x, cell.y - 1);
        Cell rightCell = gridSystem.GetGridObjectValue(cell.x + 1, cell.y);
        
        //adding found neighbors to list to randomize
        if(upCell is { isVisited: false })
            neighbors.Add(upCell);
        if(leftCell is { isVisited: false })
            neighbors.Add(leftCell);    
        if(downCell is { isVisited: false })
            neighbors.Add(downCell);  
        if(rightCell is { isVisited: false })
            neighbors.Add(rightCell);

        if (neighbors.Count > 0)
        {
            int randomIndex = Random.Range(0, neighbors.Count);
            return neighbors[randomIndex];
        }

        return null;
    } 

    /// <summary>
    /// Resets entire maze so it can be regenerated again
    /// </summary>
    public void ResetMaze()
    {
        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int y = 0; y < gridSystem.GetHeight(); y++)
            {
                Cell cell = gridSystem.GetGridObjectValue(x, y);
                cell.ResetCell();
            }
        }
        cells.Clear();
        stack.Clear();
    }
    
    /// <summary>
    /// Returns if the maze has been fully generated
    /// </summary>
    /// <returns></returns>
     public bool IsCompleted()
    {
        return stack.Count == 0;
    }
    
    /// <summary>
    /// Returns world position of the current cell
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPositionCurrentCell()
    {
        return gridSystem.GetWorldPosition(currentCell.x, currentCell.y);
    }
}
