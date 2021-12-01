using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private MazeHandler mazeHandler;
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed;
    public bool zoomCamera = false;

    private Vector3 startPosistion;
    private Vector3 newPosistion;

    private void Start()
    {
        startPosistion = transform.position;
    }

    private void Update()
    {
        if(mazeHandler.mazeGeneration.gridSystem == null)
            return;
        
        if (Mathf.Max(mazeHandler.mazeGeneration.gridSystem.GetWidth(), mazeHandler.mazeGeneration.gridSystem.GetHeight()) >= 75)
        {
            if (mazeHandler.mazeGeneration.generatingMaze && target.gameObject.activeSelf && zoomCamera)
            {
                newPosistion = new Vector3(target.position.x + 15, startPosistion.y * 0.35f, target.position.z);
                transform.position = Vector3.Lerp(transform.position, newPosistion, followSpeed * Time.deltaTime); 
            }
            else
                transform.position = startPosistion;
        }
        
    }
}
