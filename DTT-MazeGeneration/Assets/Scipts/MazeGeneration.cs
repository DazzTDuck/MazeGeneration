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
                if(x == 0)
                    upWallIndex = currentCellIndex * 2 + x;
                else if(x == gridSystem.GetWidth() - 1)
                    upWallIndex = currentCellIndex * 2 + x + y + 1;     
                else
                    upWallIndex = currentCellIndex * 2 + x + 1;

                //calculate left index
                var leftWallIndex = upWallIndex + 1;
                
                //calculate down index
                if(y == 1 && x == gridSystem.GetWidth() - 1)
                    downWallIndex = upWallIndex - 4;
                else if(y == 1 && x == gridSystem.GetWidth() - 1)
                    downWallIndex = upWallIndex - 3;
                else if(x == 0)
                    downWallIndex = upWallIndex + 2; 
                else
                    downWallIndex = upWallIndex - 2;

                //calculate right index
                if(x == gridSystem.GetWidth() - 1)
                    rightWallIndex = upWallIndex + 2;
                else
                    rightWallIndex = upWallIndex + (gridSystem.GetWidth() * 2) + 2;

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
        
        Cell cellA = gridSystem.GetGridObjectValue(8, 5);
        Cell cellB = gridSystem.GetGridObjectValue(9, 5);

        BreakWallBetweenCells(cellA, cellB);
    }

    public void BreakWallBetweenCells(Cell a, Cell b)
    {
        //break wall between 2 cells given
        foreach (var wallA in a.wallsIndex)
        {
            foreach (var wallB in b.wallsIndex)
            {
                if(wallA == wallB)
                {
                    Debug.Log($"Removed Wall: {wallA}");
                    walls[wallA].SetActive(false);
                    //just disabling wallA because they're the same wall
                    return;
                }
                Debug.LogWarning($"cell {a.x},{a.y} and cell {b.x},{b.y} do not have a shared wall");
            }
        }
    }
    
    public Cell GetRandomNeighbor(Cell cell)
    {
        List<Cell> neighbors = new List<Cell>();

        //manipulate the x and y coords to get all cells next to that cell
        Cell upCell;
        Cell downCell;
        Cell leftCell;
        Cell rightCell;

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
                var cell = gridSystem.GetGridObjectValue(x, y);
                cell.ResetCell();
            }
        }
        cells.Clear();
    }
    
}
