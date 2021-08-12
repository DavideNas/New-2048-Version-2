using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class LevelSetup : MonoBehaviour
{
    // graphic element to create base grid
    public GameObject cellGrid;

    // start point to create grid
    private float gridOrigin;

    // space between cells
    public float gridSpacing;        

    // value to store size of level grid
    private int sizeOfGrid;

    // set the distance of tiles to Z axis
    private float depthOfTile;

    // Matrix to store basic grid coordinate
    public static Vector2[,] gridCoord;

    // 
    private List<float> lineCoord;

    // Scene Camera
    public Camera cam; 

    private void Start()
    {
        gridOrigin = 0;
        gridSpacing = 0.2f;
        sizeOfGrid = ControlManager.Instance.GridSize;
        depthOfTile = ControlManager.Instance.DepthTile;
        gridCoord = new Vector2[sizeOfGrid, sizeOfGrid];
        lineCoord = new List<float>();

        InitGrid();
    }

    // init grid of levels
    private void InitGrid()
    {
        float posX;
        float posY;
        gridOrigin = -(gridSpacing * sizeOfGrid)/(sizeOfGrid/2);
        SetCameraSize();

        // Create new grid on the base
        for (int i = 0; i < sizeOfGrid; i++)
        {
            
            for (int j = 0; j < sizeOfGrid; j++)
            {
                posX = gridOrigin + ((gridSpacing + 2) * i);
                posY = gridOrigin + ((gridSpacing + 2) * j);

                // Instantiate new cell on grid
                Instantiate(cellGrid, new Vector3(posX, posY, depthOfTile + 0.5f), Quaternion.identity);

                // Save position for every possible coord on the grid
                gridCoord[i, j] = new Vector2(posX, posY);
            }
        }

        for (int t = 0; t < sizeOfGrid; t++)
        {
            lineCoord.Add(gridCoord[t, 0].x);
        }
    }

    // set camera distance
    void SetCameraSize()
    {
        float aspectRatio = (Screen.height / (float)Screen.width);

        double divisore;       // variabile da usare come divisore per correggere la posizione dello stage

        cam.orthographicSize = (float) sizeOfGrid*2 + aspectRatio*2;

        if (sizeOfGrid >= 10)
        {
            divisore = 4;
        }
        else if ((sizeOfGrid >= 7) && (sizeOfGrid <= 9))
        {
            divisore = 3;
        }
        else if ((sizeOfGrid >= 4) && (sizeOfGrid <= 6)) 
        {
            divisore = 2;
        }else
        {
            divisore = 1.5;
        }

        float cameraPosX = sizeOfGrid - (aspectRatio/(float)divisore);
        float cameraPosY = sizeOfGrid - (aspectRatio/(float)divisore);
        float cameraPosZ = -10; //-1* ((float)sizeOfGrid+1) * distanceCamera * distanceCamera;

        cam.transform.position = new Vector3(cameraPosX, cameraPosY, cameraPosZ);
    }
}
