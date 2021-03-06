using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeHandler : MonoBehaviour
{
    [Header("Maze Settings")]
    [SerializeField] private float generatingLatency = 1;
    [SerializeField] private int grid_Width;
    [SerializeField] private int grid_Height;
    [Space]
    [SerializeField] private bool showSearchingCube = false;
    [Space]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject searchingCube;

    private const int WIDTH = 180;
    private const int HEIGHT = 180;
    [HideInInspector]
    public Vector2 grid_CellSize = new Vector2(1, 1);
    private Vector2 grid_Offset;

    private GridSystem mazeGridSystem;
    public MazeGeneration mazeGeneration;

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
        MazeGenerationDelay();
        ShowSearchingCube();
    }
    
    /// <summary>
    /// Generates a new maze
    /// </summary>
    public void GenerateNewMaze()
    {
        if(!mazeGeneration.generatingMaze)
        {
            UpdateGrid();
            mazeGeneration.SetupGeneration(walls);    
        }
    }
    
    /// <summary>
    /// Handles the delay of the generation loop 
    /// </summary>
    private void MazeGenerationDelay()
    {
        //delay the generation
        if(mazeGeneration.generatingMaze)
        {
            if(generatingLatency > 0)
            {
                if(mazeTimer <= 0)
                {
                    mazeTimer = generatingLatency;
                    mazeGeneration.GenerateMaze();
                }
                else
                    mazeTimer -= Time.deltaTime;    
            }
            else
                mazeGeneration.GenerateMaze();   
        }    
    }

    /// <summary>
    /// Updates the grid and makes sure grid is sized properly and walls are placed at the correct position based on the size
    /// </summary>
    public void UpdateGrid()
    {
        mazeGridSystem = null;

        float x = (float)WIDTH / grid_Width;
        float y = (float)HEIGHT / grid_Height;
        
        float changedWidth = WIDTH;
        float changedHeight = HEIGHT;

        //scaling cells properly and changing width or height to center better
        if (grid_Width > grid_Height)
        { 
            y = x;
            changedHeight /= (float)grid_Width / grid_Height;
        }
        else if(grid_Width < grid_Height)
        {
            x = y;
            changedWidth /= (float)grid_Height / grid_Width;
        }
            
        //centering grid
        grid_Offset.x = (-changedWidth + x) * 0.5f + 0.5f;
        grid_Offset.y = (-changedHeight + y) * 0.5f + 0.5f;

        grid_CellSize = new Vector2(x, y);
        mazeGridSystem = new GridSystem(grid_Width, grid_Height, grid_CellSize, grid_Offset);
        mazeGeneration.gridSystem = mazeGridSystem;
        
        if(Math.Abs(wallsWidth - grid_Width) > 0 || Math.Abs(wallsHeight - grid_Height) > 0)
        {
            wallsWidth = grid_Width;
            wallsHeight = grid_Height;
            DestroyAllWalls();
        }
        
        if(!wallsBuilt)
            BuildAllWalls();
        else
        {   
            ShowAllWalls();
        }
    }
    
    public void ShowAllWalls()
    {
        //show all walls again
        foreach (GameObject wall in walls)
        {
            if(!wall.activeSelf)
                wall.SetActive(true);            
        }    
    }
    
    public void BuildAllWalls()
    {
        for (int x = 0; x < grid_Width; x++)
        {
            for (int y = 0; y < grid_Height; y++)
            {
                Vector3 worldPos = mazeGridSystem.GetWorldPosition(x,  y);
                
                //Top wall
                GameObject topWall = Instantiate(wallPrefab, transform);
                topWall.transform.position = worldPos + new Vector3(0,topWall.transform.localScale.y / 2,grid_CellSize.y / 2);
                topWall.transform.localScale = new Vector3(grid_CellSize.x, topWall.transform.localScale.y, topWall.transform.localScale.z);
                walls.Add(topWall);

                //Left wall
                GameObject leftWall = Instantiate(wallPrefab, transform);
                leftWall.transform.position = worldPos + new Vector3(-grid_CellSize.x / 2,leftWall.transform.localScale.y / 2,0);
                leftWall.transform.localScale = new Vector3(grid_CellSize.y, leftWall.transform.localScale.y, leftWall.transform.localScale.z);
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
                    rightWall.transform.localScale = new Vector3(grid_CellSize.y, rightWall.transform.localScale.y, rightWall.transform.localScale.z);
                    rightWall.transform.eulerAngles = new Vector3(0, 90, 0);
                    walls.Add(rightWall);
                }
            }    
        }
        wallsBuilt = true;

        //setting names of walls to their index in the list to have a better reference
        for (int i = 0; i < walls.Count; i++)
        {
            walls[i].name = i.ToString();    
        }
    }
    
    private void DestroyAllWalls()
    {
        foreach (GameObject wall in walls)
        {
            Destroy(wall);    
        }
        walls.Clear();
        wallsBuilt = false;
    }
    
    private void ShowSearchingCube()
    {
        if(showSearchingCube)
        {
            if(mazeGeneration.generatingMaze)
            {
                searchingCube.transform.position = mazeGeneration.GetPositionCurrentCell();
                searchingCube.transform.localScale = new Vector3(grid_CellSize.x / 2, searchingCube.transform.localScale.y, grid_CellSize.y / 2);
            }    
        }
        
        if(!mazeGeneration.generatingMaze)
            searchingCube.SetActive(false); 
        else
        {
            if(searchingCube.activeSelf != showSearchingCube)
                searchingCube.SetActive(showSearchingCube);        
        }
    }
    
    public void StopGeneration()
    { 
        mazeGeneration.generatingMaze = false;
        mazeGeneration.ResetMaze();
        DestroyAllWalls();
    }
    
    public void SetMazeSize(int newWidth, int newHeight)
    {
        grid_Width = newWidth;
        grid_Height = newHeight;
    }
    public void SetLatency(float newLatency)
    {
        generatingLatency = newLatency;
    }
    public void ShowGenerationBetter(bool showGeneration)
    {
        showSearchingCube = showGeneration;       
    }
}
