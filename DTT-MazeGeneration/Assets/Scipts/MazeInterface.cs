using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MazeInterface : MonoBehaviour
{
    [SerializeField] private CameraFollow cameraFollow;
    [Space]
    [SerializeField] private ValueChanger widthValue;
    [SerializeField] private ValueChanger heightValue;
    [SerializeField] private ValueChanger latencyValue;
    [SerializeField] private Toggle showSearchingCube;
    [SerializeField] private Toggle optimizeGeneration;
    
    private MazeHandler mazeHandler;

    private void Start()
    {
        mazeHandler = GetComponent<MazeHandler>();
        
        widthValue.SetButtonListeners(UpdateMazeSize);
        heightValue.SetButtonListeners(UpdateMazeSize);
        latencyValue.SetButtonListeners(SetLatency);
    }
    
    public void UpdateMazeSize()
    {
        mazeHandler.SetMazeSize((int)widthValue.CurrentValue, (int)heightValue.CurrentValue);
    }
    
    public void SetLatency()
    {
        mazeHandler.SetLatency(latencyValue.CurrentValue);
    }

    public void SetZoomCamera(bool canZoom)
    {
        cameraFollow.zoomCamera = canZoom;    
    }
}
