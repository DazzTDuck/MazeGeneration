using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MazeGeneration
{
    public Grid<Cell> grid {get; set;}
    public bool generatingMaze {get; set;}

    private List<Cell> visitedCells = new List<Cell>();

    public MazeGeneration(Grid<Cell> grid, bool generatingMaze)
    {
        this.grid = grid;
        this.generatingMaze = generatingMaze;
    }

    public void GenerateMaze()
    {
        Debug.Log("generating");

        //select random starting cell
        SetStartCell();
        //select random end cell;

        //done generating
        generatingMaze = false;
    }

    public void SetStartCell()
    {
        RandomizeCell(out var randomCell);
        randomCell.startCell = true;
    }

    public void RandomizeCell(out Cell cell)
    {
        int randomX = Random.Range(0, grid.GetWidth());
        int randomY = Random.Range(0, grid.GetHeight());

        cell = grid.GetGridObjectValue(randomX, 0);
    }
}
