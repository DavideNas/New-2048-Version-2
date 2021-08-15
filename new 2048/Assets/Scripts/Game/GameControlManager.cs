using System;
using System.Collections.Generic;
using UnityEngine;

public class GameControlManager : MonoBehaviour
{
    public static GameControlManager Instance { get; private set; }

    private bool prevScoreSaved;

    public float velocity;

    private int continueCount;
    public int ContinueCount{
        get { return continueCount; }
        set { continueCount = value; }
    }

    private bool undoReady;
    public bool UndoReady{ 
        get { return undoReady; }
        set { undoReady = value; }
    }

    // 
    public int moveCount;

    // actual points of game stage
    private float score;                 

    // previous points of game stage
    private float prevScore;

    public List<float> allScoreStack;

    // info for all tiles on stage
    public List<Vector3> allTilesPositions;
    public List<float> allTilesValue;

    // info for Undo check
    public static List<Vector3> allTilesPositionsUndo;
    public static List<float> allTilesValueUndo;

    public static List<List<float>> allTilesValueStack;
    public static List<List<Vector3>> allTilesPositionsStack;

    // list to manage tiles on stage
    public List<GameObject> tilesOnGrid;

    private void Start()
    {
        if (null == Instance)
            Instance = this;

        prevScoreSaved = false;
        score = 0f;
        prevScore = 0f;

        moveCount = 0;

        velocity = 20f;

        ContinueCount = 0;

        allScoreStack = new List<float>();
        allTilesValueStack = new List<List<float>>();
        allTilesPositionsStack = new List<List<Vector3>>();

        allTilesPositions = new List<Vector3>();
        allTilesValue = new List<float>();
        tilesOnGrid = new List<GameObject>();
        
        UndoReady = false;
        allTilesPositionsUndo = new List<Vector3>();
        allTilesValueUndo = new List<float>();


    }

    // update points on stage with zeros
    public string UpdateScore(float points)
    {
        if (!prevScoreSaved)
        {
            prevScore = score;
            prevScoreSaved = true;
        }

        string scoreUpdated = "";
        score += points / 2;

        allScoreStack.Add(score);                               // save score into list for Undo recover

        scoreUpdated += score.ToString();

        float recordcheck = float.Parse(UIManager.Instance.ScoreRecord.ToString());
        if (Math.Abs(recordcheck) <= Math.Abs(score))
        {
            long newRecord = Convert.ToInt64(score);

            if (3 == ControlManager.Instance.GridSize)
                PlayGamesScript.AggiungiPunteggioClassifica3x3(newRecord);
            if (4 == ControlManager.Instance.GridSize)
                PlayGamesScript.AggiungiPunteggioClassifica4x4(newRecord);
            if (5 == ControlManager.Instance.GridSize)
                PlayGamesScript.AggiungiPunteggioClassifica5x5(newRecord);
            if (6 == ControlManager.Instance.GridSize)
                PlayGamesScript.AggiungiPunteggioClassifica6x6(newRecord);
            if (8 == ControlManager.Instance.GridSize)
                PlayGamesScript.AggiungiPunteggioClassifica8x8(newRecord);

            UIManager.Instance.ScoreRecord = score.ToString();
        }

        return scoreUpdated;
    }

    // function to read and save all tiles info in the stage
    public void CheckTilesInfo()
    {
        foreach (GameObject tileOnStage in tilesOnGrid)
        {
            if (tileOnStage != null)
            {
                allTilesValue.Add(tileOnStage.GetComponent<Tile>().tilePoints);                     // save each value of tiles
                allTilesPositions.Add(tileOnStage.GetComponent<Tile>().transform.position);         // save each positions of tiles
            }
        }
    }

    public void SaveTileStack()
    {
        CountTiles();

        CheckTilesInfo();

        allTilesValueStack.Add(new List<float>(allTilesValueUndo));
        allTilesPositionsStack.Add(new List<Vector3>(allTilesPositionsUndo));

        allTilesValueUndo.Clear();
        allTilesPositionsUndo.Clear();

        foreach (GameObject tileOnStage in tilesOnGrid)
        {
            if (tileOnStage != null)
            {
                allTilesValueUndo.Add(tileOnStage.GetComponent<Tile>().tilePoints);                     // save each value of tiles
                allTilesPositionsUndo.Add(tileOnStage.GetComponent<Tile>().transform.position);         // save each positions of tiles
            }
        }
    }

    // load function for Undo skill usage
    public void LoadPrevStage()
    {
        UIManager.Instance.ActualPoints = prevScore.ToString();
        score = prevScore;

        if (allTilesValueStack.Count > 1)
        {
            foreach (GameObject singleTile in tilesOnGrid)    // Clean all stage from Tiles
            {
                if (singleTile != null)
                {
                    Destroy(singleTile.gameObject);
                }
            }

            allTilesValueUndo.Clear();
            allTilesPositionsUndo.Clear();
            
            allTilesValueUndo = new List<float>(allTilesValueStack[allTilesValueStack.Count - 1]);
            allTilesPositionsUndo = new List<Vector3>(allTilesPositionsStack[allTilesPositionsStack.Count - 1]);

            allTilesValueStack.RemoveAt(allTilesValueStack.Count - 1);
            allTilesPositionsStack.RemoveAt(allTilesPositionsStack.Count - 1);


            string loadedStackValues = "";

            for (int t = 0; t < allTilesValueUndo.Count; t++)
            {
                string tileV = allTilesValueUndo[t] + " - ";
                loadedStackValues += tileV;

                string tile = ControlManager.Instance.TileSelect;                           // Load Tile type
                GameObject loadedTile = (GameObject)Instantiate(Resources.Load(tile, typeof(GameObject)), allTilesPositionsUndo[t], Quaternion.identity);
                loadedTile.GetComponent<Tile>().targetPosition = allTilesPositionsUndo[t];
                // Load Tile in a his own position

                loadedTile.GetComponent<Tile>().tilePoints = allTilesValueUndo[t];
                // Set point value for loaded tile

                loadedTile.GetComponent<Tile>().tileValue.text = allTilesValueUndo[t].ToString();
                // Set Value for loaded Tile

                loadedTile.GetComponent<Tile>().SetColor(allTilesValueUndo[t]);
                // Set Color for loaded Tile
            }
        }
        /*else
        {
            Debug.Log("System doesn't have any previous state to load");
        }*/
    }

    // delete all tiles from grid
    public void ClearStage()
    {
        score = 0f;
        // Clean all stage from Tiles
        foreach (GameObject singleTile in tilesOnGrid)          
        {
            if (singleTile != null)
            {
                Destroy(singleTile.gameObject);
            }
        }
    }

    public void CountTiles()                     // 2 of 2 -> monobehaviour inherit class
    {
        try
        {
            GameObject[] allTiles = (GameObject[])FindObjectsOfType(typeof(GameObject));
            //ResetMaps();
            Instance.tilesOnGrid.Clear();
            for (int k = 0; k < allTiles.Length; k++)
            {
                if (k < allTiles.Length)
                {
                    // Save tiles in a list
                    if (allTiles[k].GetComponent<Tile>())
                    {
                        Instance.tilesOnGrid.Add(allTiles[k]);
                    }
                }
            }

            //Debug.Log("The stage has been saved on List");
        }
        catch (IndexOutOfRangeException ex)
        {
            Debug.Log("An Error Of Overlay Tile Occurred During Count : " + ex);
        }
    }
}