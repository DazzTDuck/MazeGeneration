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
    public Grid grid {get; set;}
    public bool generatingMaze = false;

    private List<Cell> visitedCells = new List<Cell>();

    public MazeGeneration(Grid grid, bool generatingMaze)
    {
        this.grid = grid;
        this.generatingMaze = generatingMaze;
    }

    public void GenerateMaze()
    {
        ResetMaze();
        Debug.Log("generating");

        //select random starting cell
        SetStartCell();
        //select random end cell

        //done generating
        generatingMaze = false;
    }

    public void SetStartCell()
    {
        RandomizeCell(out var randomCell);
        randomCell.startCell = true;
        randomCell.isWall = false;
    }

    public void RandomizeCell(out Cell cell)
    {
        if (Random01(1, 0) == 1)
        {
            int randomY = Random.Range(0, grid.GetHeight() - 1);

            cell = grid.GetGridObjectValue(Random01(0, grid.GetWidth() - 1), randomY);
        }
        else
        {
            int randomX = Random.Range(0, grid.GetWidth() - 1);

            cell = grid.GetGridObjectValue(randomX, Random01(0, grid.GetHeight() - 1));
        }
    }

    public void ResetMaze()
    {
        bool wall = false;

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                var cell = grid.GetGridObjectValue(x, y);
                cell.ResetCell();
                cell.isWall = wall;
                wall = !wall;

                //Debug.Log(cell.GetID());
            }    
        }
    }

    public static int Random01(int trueValue, int falseValue)
    {
        int randomSide = Random.Range(0, 2);
        bool hit = randomSide == 1;

        return hit ? trueValue : falseValue;
    }
}
