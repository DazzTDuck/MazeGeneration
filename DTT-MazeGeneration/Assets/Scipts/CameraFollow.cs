using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] MazeHandler mazeHandler;
    [SerializeField] Transform target;
    [SerializeField] float followSpeed;

    private Vector3 startPosistion;
    private Vector3 newPosistion;

    private void Start()
    {
        startPosistion = transform.position;
    }

    private void Update()
    {
        if (mazeHandler.mazeGeneration.generatingMaze && target.gameObject.activeSelf)
        {
            newPosistion = new Vector3(target.position.x, startPosistion.y * Mathf.Clamp01(mazeHandler.grid_CellSize.x * 0.3f), target.position.z);
            transform.position = Vector3.Lerp(transform.position, newPosistion, followSpeed * Time.deltaTime); 
        }
        else
            transform.position = startPosistion;
    }
}
