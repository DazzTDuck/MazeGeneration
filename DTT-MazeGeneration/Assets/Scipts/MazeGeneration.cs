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

    public void SetWallIndexesToCells()
    {
        //if(x == 0): cellIndex * 2 + X == UpWallIndex
        //else if(x == width - 1): cellIndex * 2 + X + Y + 1 == UpWallIndex
        //else: cellIndex * 2 + X + 1 == UpWallIndex

        //UpWallIndex + 1 == LeftWallIndex

        //if(y == 1 && x == gridWidth - 1): UpWallIndex - 4 == DownWallIndex
        //else if(y == 1 || x == gridWidth - 1):UpWallIndex - 3 == DownWallIndex
        //else if(x == 0): UpWllIndex + 2 == DownWallIndex
        //else: UpWallIndex - 2 == DownWallIndex

        //if(x == gridWidth - 1): UpWallIndex + 2 == RightWallIndex
        //else: UpWallIndex + (width * 2) + 2 == RightWallIndex

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
                // if(x == 0)
                //     upWallIndex = currentCellIndex * 2 + x;
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
    
    public void GenerateMaze()
    {
        Debug.Log("generating");

        //for debugging wall indexes to see why they're not correct
        // Cell cellA = gridSystem.GetGridObjectValue(19, 7);
        // Cell cellB = gridSystem.GetGridObjectValue(18, 7);
        //
        // RemoveWallBetweenCells(cellA, cellB);
        // return;
        
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

    public void RemoveWallBetweenCells(Cell a, Cell b)
    {
        //debug wallIndexes if not correct
        // foreach (int wallA in a.wallsIndex)
        // {
        //     Debug.Log($"{a.x},{a.y} = {wallA}");    
        // }
        // foreach (int wallB in b.wallsIndex)
        // {
        //     Debug.Log($"{b.x},{b.y} = {wallB}");
        // }

        //break wall between 2 cells given
        foreach (int wallA in a.wallsIndex)
        {
            foreach (int wallB in b.wallsIndex)
            {
                if(wallA == wallB)
                {
                    //Debug.Log($"Removed Wall: {wallA}");
                    walls[wallA].SetActive(false);
                    //just disabling wallA because they're the same wall
                    return;
                }
            }
        }
        Debug.LogWarning($"cell {a.x},{a.y} and cell {b.x},{b.y} does not have a shared wall");
    }
    
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
    }
    
     public bool IsCompleted()
    {
        return stack.Count == 0;
    }
     
    public Vector3 GetPositionCurrentCell()
    {
        return gridSystem.GetWorldPosition(currentCell.x, currentCell.y); 
    }
}
