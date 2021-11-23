using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

public class MazeHandler : MonoBehaviour
{
    [Header("Maze Settings")]
    [SerializeField] private float generatingLatency = 1;
    [SerializeField] private int grid_Width;
    [SerializeField] private int grid_Height;
    [Space]
    [SerializeField] private GameObject wallPrefab;

    private const int width = 25;
    private const int height = 25;
    private Vector2 grid_CellSize = new Vector2(1, 1);
    private Vector2 grid_Offset;

    private GridSystem mazeGridSystem;
    private MazeGeneration mazeGeneration;

    private float mazeTimer = 0;
    private bool wallsBuilt = false;
    
    private List<GameObject> walls = new List<GameObject>();
    private float wallsWidth;
    private float wallsHeight;

    private void Start()
    {
        mazeGeneration = new MazeGeneration(mazeGridSystem, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !mazeGeneration.generatingMaze)
        {
            //mazeGeneration.generatingMaze = true;
            UpdateGrid();
            //mazeGeneration.SetupGeneration();
        }
        
        //delay the generation
        if(mazeGeneration.generatingMaze)
        {
            if(mazeTimer <= 0)
            {
                mazeTimer = generatingLatency;
                mazeGeneration.GenerateMaze();
            }
            else
                mazeTimer -= Time.deltaTime;
        }
    }

    public void UpdateGrid()
    {
        mazeGridSystem = null;

        grid_Offset.x = -width * 0.5f + 0.5f + transform.position.x;
        grid_Offset.y = -height * 0.5f + 0.5f;

        float x = (float)width / grid_Width;
        float y = (float)height / grid_Height;

        grid_CellSize = new Vector2(x, y);
        mazeGridSystem = new GridSystem(grid_Width, grid_Height, grid_CellSize, grid_Offset);
        mazeGeneration.gridSystem = mazeGridSystem;
        
        if(Math.Abs(wallsWidth - grid_Width) > 0 || Math.Abs(wallsHeight - grid_Height) > 0)
        {
            wallsBuilt = false;
            wallsWidth = grid_Width;
            wallsHeight = grid_Height;
            DestroyAllWalls();
        }
        
        if(!wallsBuilt)
            BuildAllWalls();
        else
        {
            //show all walls again
            foreach (GameObject wall in walls)
            {
                if(!wall.activeSelf)
                    wall.SetActive(true);            
            }    
        }
    }
    
    public void BuildAllWalls()
    {
        for (int x = 0; x < grid_Width; x++)
        {
            for (int y = 0; y < grid_Height; y++)
            {
                Cell cell = mazeGridSystem.GetGridObjectValue(x, y);
                Vector3 worldPos = mazeGridSystem.GetWorldPosition(x,  y);
                
                //Top wall
                GameObject topWall = Instantiate(wallPrefab, transform);
                topWall.transform.position = worldPos + new Vector3(0,topWall.transform.localScale.y / 2,grid_CellSize.y / 2);
                topWall.transform.localScale = new Vector3(grid_CellSize.x, topWall.transform.localScale.y, topWall.transform.localScale.z);
                walls.Add(topWall);
                
                //Left wall
                GameObject leftWall = Instantiate(wallPrefab, transform);
                leftWall.transform.position = worldPos + new Vector3(-grid_CellSize.x / 2,leftWall.transform.localScale.y / 2,0);
                leftWall.transform.localScale = new Vector3(grid_CellSize.x, leftWall.transform.localScale.y, leftWall.transform.localScale.z);
                leftWall.transform.eulerAngles = new Vector3(0, 90, 0);
                walls.Add(leftWall);
                
                if(y == 0)
                {
                    //Bottom wall
                    GameObject bottomWall = Instantiate(wallPrefab, transform);
                    bottomWall.transform.position = worldPos + new Vector3(0,bottomWall.transform.localScale.y / 2,-grid_CellSize.y / 2);
                    bottomWall.transform.localScale = new Vector3(grid_CellSize.x, bottomWall.transform.localScale.y, bottomWall.transform.localScale.z);
                    walls.Add(bottomWall);
                }
                
                if(x == grid_Width - 1)
                {
                    //Right wall
                    GameObject rightWall = Instantiate(wallPrefab, transform);
                    rightWall.transform.position = worldPos + new Vector3(grid_CellSize.x / 2,rightWall.transform.localScale.y / 2,0);
                    rightWall.transform.localScale = new Vector3(grid_CellSize.x, rightWall.transform.localScale.y, rightWall.transform.localScale.z);
                    rightWall.transform.eulerAngles = new Vector3(0, 90, 0);
                    walls.Add(rightWall);
                }
            }    
        }
        wallsBuilt = true;
    }
    
    private void DestroyAllWalls()
    {
           
    }

    private void OnDrawGizmosSelected()
    {
        //draw grid
        if (mazeGridSystem == null)
            return;
        
        for (int x = 0; x < mazeGridSystem.GetWidth(); x++)
        {
            for (int y = 0; y < mazeGridSystem.GetHeight(); y++)
            {
                Cell currentCell = mazeGridSystem.GetGridObjectValue(x, y);
                
                Gizmos.color = Color.white;
                
                Gizmos.DrawCube(mazeGridSystem.GetWorldPosition(x, y), new Vector3(grid_CellSize.x, 0, grid_CellSize.y));
            }    
        }
    }
}
