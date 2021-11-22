using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeHandler : MonoBehaviour
{
    [Header("Maze Settings")] 
    [SerializeField] private int grid_Width;
    [SerializeField] private int grid_Height;

    private const int width = 25;
    private const int height = 25;
    private Vector2 grid_CellSize = new Vector2(1, 1);
    private Vector2 grid_Offset;

    private GridSystem _mazeGridSystem;
    private MazeGeneration mazeGeneration;


    private void Start()
    {
        mazeGeneration = new MazeGeneration(_mazeGridSystem, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !mazeGeneration.generatingMaze)
        {
            mazeGeneration.generatingMaze = true;
            UpdateGrid();
            mazeGeneration.SetupGeneration();
        }
    }

    public void UpdateGrid()
    {
        _mazeGridSystem = null;

        grid_Offset.x = -width * 0.5f + 0.5f + transform.position.x;
        grid_Offset.y = -height * 0.5f + 0.5f;

        float x = (float)width / grid_Width;
        float y = (float)height / grid_Height;

        grid_CellSize = new Vector2(x, y);
        _mazeGridSystem = new GridSystem(grid_Width, grid_Height, grid_CellSize, grid_Offset);
        mazeGeneration.gridSystem = _mazeGridSystem;
    }

    private void OnDrawGizmosSelected()
    {
        //draw grid
        if (_mazeGridSystem == null)
            return;

        for (int x = 0; x < grid_Width; x++)
        {
            for (int y = 0; y < grid_Height; y++)
            {
                Cell currentCell = _mazeGridSystem.GetGridObjectValue(x, y);

                Gizmos.color = currentCell.isWall ? Color.black : currentCell.isVisited ? Color.yellow : Color.white;

                Gizmos.color = currentCell.isWall ? Color.black  
                    : currentCell.startCell ? Color.green : currentCell.endCell ? Color.red : Color.white;

                Gizmos.DrawCube(_mazeGridSystem.GetWorldPosition(x, y), new Vector3(grid_CellSize.x, 0, grid_CellSize.y));
            }    
        }
    }
}
