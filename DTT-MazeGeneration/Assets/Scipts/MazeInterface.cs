using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeInterface : MonoBehaviour
{
    [SerializeField] private ValueChanger widthValue;
    [SerializeField] private ValueChanger heightValue;
    [SerializeField] private ValueChanger latencyValue;
    [SerializeField] private Toggle showSearchingCube;
    [SerializeField] private Toggle optimizeGeneration;
    
    private MazeHandler mazeHandler;

    private void Start()
    {
        mazeHandler = GetComponent<MazeHandler>();
    }
}
