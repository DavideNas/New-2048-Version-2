using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class LevelSetup : MonoBehaviour
{
    public static LevelSetup Instance { get; private set; }

    // graphic element to create base grid
    public GameObject cellGrid;

    // start point to create grid
    private float gridOrigin;

    // space between cells
    public float gridSpacing;

    // Matrix to store basic grid coordinate
    public Vector2[,] gridCoord;
    
    // 
    public List<float> lineCoord;

    // Scene Camera
    public Camera cam;

    private void Start()
    {
        if( null == Instance )
            Instance = this;

        gridOrigin = 0;
        gridSpacing = 0.2f;
        gridCoord = new Vector2[ControlManager.Instance.GridSize, ControlManager.Instance.GridSize];
        lineCoord = new List<float>();

        InitGrid();
    }

    // init grid of levels
    private void InitGrid()
    {
        float posX;
        float posY;
        gridOrigin = -(gridSpacing * ControlManager.Instance.GridSize)/(ControlManager.Instance.GridSize/2);
        SetCameraSize();

        // Create new grid on the base
        for (int i = 0; i < ControlManager.Instance.GridSize; i++)
        {
            for (int j = 0; j < ControlManager.Instance.GridSize; j++)
            {
                posX = gridOrigin + ((gridSpacing + 2) * i);
                posY = gridOrigin + ((gridSpacing + 2) * j);

                // Instantiate new cell on grid
                Instantiate(cellGrid, new Vector3(posX, posY, ControlManager.Instance.DepthTile + 0.5f), Quaternion.identity);

                // Save position for every possible coord on the grid
                gridCoord[i, j] = new Vector2(posX, posY);
            }
        }

        for (int t = 0; t < ControlManager.Instance.GridSize; t++)
        {
            lineCoord.Add(gridCoord[t, 0].x);
        }
    }

    // set camera distance
    public void SetCameraSize()
    {
        float aspectRatio = (Screen.height / (float)Screen.width);

        double divisore;       // variabile da usare come divisore per correggere la posizione dello stage

        cam.orthographicSize = (float) ControlManager.Instance.GridSize*2 + aspectRatio*2;

        if (ControlManager.Instance.GridSize >= 10)
        {
            divisore = 4;
        }
        else if ((ControlManager.Instance.GridSize >= 7) && (ControlManager.Instance.GridSize <= 9))
        {
            divisore = 3;
        }
        else if ((ControlManager.Instance.GridSize >= 4) && (ControlManager.Instance.GridSize <= 6)) 
        {
            divisore = 2;
        }else
        {
            divisore = 1.5;
        }

        float cameraPosX = ControlManager.Instance.GridSize - (aspectRatio/(float)divisore);
        float cameraPosY = ControlManager.Instance.GridSize - (aspectRatio/(float)divisore);
        float cameraPosZ = -10; //-1* ((float)sizeOfGrid+1) * distanceCamera * distanceCamera;

        cam.transform.position = new Vector3(cameraPosX, cameraPosY, cameraPosZ);
    }

    public void RestoreStage()
    {
        //ScoreText.text = Utils.UpdateScore(Utils.score);            // Load Score

        for (int t = 0; t < GameControlManager.Instance.allTilesValue.Count; t++)
        {
            string tile = "ClassicTile"; //Utils.tileSelect;                // Load Tile type
            GameObject loadedTile = (GameObject)Instantiate(Resources.Load(tile, typeof(GameObject)), GameControlManager.Instance.allTilesPositions[t], Quaternion.identity);
            loadedTile.GetComponent<Tile>().targetPosition = GameControlManager.Instance.allTilesPositions[t];
                                                                            // Load Tile in a his own position

            loadedTile.GetComponent<Tile>().tilePoints = GameControlManager.Instance.allTilesValue[t];
                                                                            // Set point value for loaded tile
                                                                            
            loadedTile.GetComponent<Tile>().tileValue.text = GameControlManager.Instance.allTilesValue[t].ToString();
                                                                            // Set Value for loaded Tile                                                             

            //loadedTile.GetComponent<Tile>().SetColor(GameControlManager.Instance.allTilesValue[t]);
                                                                            // Set Color for loaded Tile
        }
        GameControlManager.Instance.CountTiles();                           // Update stage state
    }
}