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

    private int newMove;
    public int NewMove{ 
        get { return newMove; }
        set { newMove = value; }    
    }

    private float randX;               // save X position to spawn new tile
    private float randY;               // save Y position to spawn new tile

    // contains all free position of the grid
    private List<Vector2> voidOnGrid;

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

    // lists to manipulate rows and columns
    private static List<GameObject>[] rowGrid;
    private static List<GameObject>[] colGrid;

    public static Vector2[,] gridCoord;

    private void Start()
    {
        GC.Collect();

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

        rowGrid = new List<GameObject>[ControlManager.Instance.GridSize];
        colGrid = new List<GameObject>[ControlManager.Instance.GridSize];

        voidOnGrid = new List<Vector2>();

        randX = 0f;
        randY = 0f;

        gridCoord = new Vector2[ControlManager.Instance.GridSize, ControlManager.Instance.GridSize];

        if(ControlManager.Instance.NewGame)
        {
            for (int x = 0; x < ControlManager.Instance.StartTilesNo; x++)
            {
                Invoke("GenerateNewTile", ControlManager.Instance.SpawnDelayTime + (x * (ControlManager.Instance.SpawnDelayTime / ControlManager.Instance.StartTilesNo)));
            }
        }
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
    
    public void UpdateTilesList()
    {
        CountTiles();
        try
        {
            for (int r = 0; r < ControlManager.Instance.GridSize; r++)
            {
                rowGrid[r].Clear();
            }
            for (int c = 0; c < ControlManager.Instance.GridSize; c++)
            {
                colGrid[c].Clear();
            }
            foreach (GameObject singleTile in tilesOnGrid)
            {
                MapRows(singleTile);
                MapColumns(singleTile);
            }
        }
        catch (IndexOutOfRangeException ex)
        {
            Debug.Log("An Error Of Overlay Tile Occurred During Update Of Tile List : " + ex);
            OutOfRangeErrorFix();
        }
        OrderRowAndGrid();
    }

    void OrderRowAndGrid()
    {
        try
        {
            for (int y = 0; y < ControlManager.Instance.GridSize; y++)
            {
                List<GameObject> newSortedRow = new List<GameObject>();
                newSortedRow = RowListToSort(rowGrid[y]);
                rowGrid[y] = newSortedRow;
            }
            for (int x = 0; x < ControlManager.Instance.GridSize; x++)
            {
                List<GameObject> newSortedColumn = new List<GameObject>();
                newSortedColumn = ColumnListToSort(colGrid[x]);
                colGrid[x] = newSortedColumn;
            }
        }
        catch (IndexOutOfRangeException ex)
        {
            Debug.Log("An Error Occur During Order Function : "+ ex);
            OutOfRangeErrorFix();
        }
    }
    
    void MapRows(GameObject thisTileInRow)
    {
        try
        {
            int wichRow = Find_Y(thisTileInRow);
            if (rowGrid[wichRow].Count - 1 < ControlManager.Instance.GridSize)
            {
                rowGrid[wichRow].Add(thisTileInRow);
            }
        }
        catch (IndexOutOfRangeException ex)
        {
            Debug.Log("An Error Of Overlay Tile Occurred During Map Of Row Function: " + ex);
            OutOfRangeErrorFix();
        }
    }

    void MapColumns(GameObject thisTileInColumn)
    {
        try
        {
            int wichColumn = Find_X(thisTileInColumn);
            colGrid[wichColumn].Add(thisTileInColumn);
        }
        catch (IndexOutOfRangeException ex)
        {
            Debug.Log("An Error Of Overlay Tile Occurred During Map Of Row Function: " + ex);
            OutOfRangeErrorFix();
        }
    }

    // Convert coord in grid position - X line
    public int Find_X(GameObject tileObject) 
    {
        int xFound = -1;
        for (int x = 0; x < ControlManager.Instance.GridSize; x++)
        {
            for (int y = 0; y < ControlManager.Instance.GridSize; y++)
            {
                if ((tileObject.transform.position.x == gridCoord[x, y].x) && (tileObject.transform.position.y == gridCoord[x, y].y))
                    xFound = x;
            }
        }
        return xFound;
    }

    // Convert coord in grid position - Y line
    public int Find_Y(GameObject tileObject)
    {
        int yFound = -1;
        for (int x = 0; x < ControlManager.Instance.GridSize; x++)
        {
            for (int y = 0; y < ControlManager.Instance.GridSize; y++)
            {
                if ((tileObject.transform.position.x == gridCoord[x, y].x) && (tileObject.transform.position.y == gridCoord[x, y].y))
                    yFound = y;
            }
        }
        return yFound;
    }

    // 1 of 2 -> monobehaviour inherit class
    void OutOfRangeErrorFix()
    {
        List<Vector3> savedPos = new List<Vector3>();
        foreach (GameObject aTile in tilesOnGrid)
        {
            savedPos.Add(aTile.transform.position);
            for (int v = 0; v < savedPos.Count - 1; v++)
            {
                if (savedPos[v] == aTile.transform.position)
                {
                    GC.Collect();
                    Destroy(aTile.gameObject);
                }
            }
        }
    }

    // 2 of 2 -> monobehaviour inherit class
    public void CountTiles()
    {
        try
        {
            GameObject[] allTiles = (GameObject[])FindObjectsOfType(typeof(GameObject));
            //ResetMaps();
            tilesOnGrid.Clear();
            for (int k = 0; k < allTiles.Length; k++)
            {
                if (k < allTiles.Length)
                {
                    // Save tiles in a list
                    if (allTiles[k].GetComponent<Tile>())
                    {
                        tilesOnGrid.Add(allTiles[k]);
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

        // Save void cells to extract new position
    public void CheckFreePos()
    {
        try
        {
            // Reset list of void
            voidOnGrid.Clear();

            // Fill voidOnGrid with element from gridCoord
            foreach (Vector2 coordToCompare in gridCoord)
                voidOnGrid.Add(coordToCompare);

            // Scan every tile on stage
            foreach (GameObject myTile in tilesOnGrid)
            {
                if (myTile != null)
                {
                    // Scan every position on grid 
                    for (int k = 0; k < voidOnGrid.Count; k++)
                    {
                        // If position of grid match with one of a tile 
                        if ((voidOnGrid[k].x == myTile.transform.position.x) && (voidOnGrid[k].y == myTile.transform.position.y))
                        {
                            // Remove from list of void cells
                            voidOnGrid.RemoveAt(k);
                            break;
                        }
                    }
                }
            }
        }
        catch (IndexOutOfRangeException ex)
        {
            Debug.Log("An Error Of Overlay Tile Occurred During §Check Free Pos Function : " + ex);
            OutOfRangeErrorFix();
        }
    }

    public void UpdateGrid()
    {
        try
        {
            int conta = 0;

            List<Vector3> tilesToRemove = new List<Vector3>();
            // try for Loop to control better number of iteration, Must skip if evolve because it will iterate one less time
            foreach (GameObject selectedTile in tilesOnGrid)
            {
                conta++;
                int tileMatching = 0;
                foreach (GameObject comparedTile in tilesOnGrid)
                {
                    if (selectedTile.GetComponent<Tile>().targetPosition == comparedTile.GetComponent<Tile>().targetPosition)
                    {
                        tileMatching++;
                        if (tileMatching >= 2)
                        {
                            tilesToRemove.Add(selectedTile.GetComponent<Tile>().targetPosition);
                            break;
                        }
                    }
                }
            }
            foreach (Vector3 evolutionCell in tilesToRemove)
            {
                EvolveCellTiles(evolutionCell);
            }
        }
        catch (IndexOutOfRangeException ex)
        {
            Debug.Log("An Error Of Overlay Tile Occurred During Update Of Grid : " + ex);
            OutOfRangeErrorFix();
        }

        UpdateTilesList();
    }

        // return random X and Y position
    public Vector3 GetRandomLocationForNewTile()
    {
        CountTiles();
        CheckFreePos();
        // *estraggo una cella libera dalla griglia
        int randIndex = UnityEngine.Random.Range(0, voidOnGrid.Count);

        // *coordinata x della cella libera
        randX = voidOnGrid[randIndex].x;

        // *coordinata y della cella libera
        randY = voidOnGrid[randIndex].y;

        return new Vector3(randX, randY, ControlManager.Instance.DepthTile);
    }

    //List<GameObject> LineToSort(List<GameObject> myShuffleList)
    public List<GameObject> RowListToSort(List<GameObject> myRowList)
    {
        if (myRowList.Count > 0)
        {
            // It get single element to compare with next one on the list
            // Sort of a list
            GameObject tempTile;
            // Algoritmo di bubble sort
            for (int i = 0; i < myRowList.Count - 1; i++)
            {
                for (int k = 0; k < myRowList.Count - 1; k++)
                {
                    // Confronto la coordinata x dell'oggetto alla posizione k con quella alla posizione k+1
                    if (myRowList[k].transform.position.x > myRowList[k + 1].transform.position.x)
                    {
                        tempTile = myRowList[k];
                        myRowList[k] = myRowList[k + 1];
                        myRowList[k + 1] = tempTile;
                    }
                }
            }
            return myRowList;
        }
        else
        {
            return new List<GameObject>();
        }
    }

    public List<GameObject> ColumnListToSort(List<GameObject> myColumnList)
    {
        if (myColumnList.Count > 0)
        {
            // It get single element to compare with next one on the list
            // Sort of a list
            GameObject tempTile;
            // Algoritmo di bubble sort
            for (int i = 0; i < myColumnList.Count - 1; i++)
            {
                for (int k = 0; k < myColumnList.Count - 1; k++)
                {
                    // Confronto la coordinata x dell'oggetto alla posizione k con quella alla posizione k+1
                    if (myColumnList[k].transform.position.y > myColumnList[k + 1].transform.position.y)
                    {
                        tempTile = myColumnList[k];
                        myColumnList[k] = myColumnList[k + 1];
                        myColumnList[k + 1] = tempTile;
                    }
                }
            }
            return myColumnList;
        }
        else
        {
            return new List<GameObject>();
        }
    }

    public void EvolveCellTiles(Vector3 newTilePos)
    {
        try
        {
            float tileToEvolveValue = 0f;
            // Scan all tiles searching the matching ones with same position
            
            foreach (GameObject tileToEvolve in tilesOnGrid)
            {
                if (tileToEvolve.GetComponent<Tile>().targetPosition == newTilePos)
                {
                    // Read point of tile to evolve
                    tileToEvolveValue = tileToEvolve.GetComponent<Tile>().tilePoints;

                    Destroy(tileToEvolve.gameObject);
                }
            }

            tileToEvolveValue *= 2;

            // Calculate new tile to replace 2 before ones
            string tile = ControlManager.Instance.TileSelect;

            // Instantiate new tile and assign position from the prev tiles , by reading "newTilePos" variable
            GameObject newTile = (GameObject)Instantiate(Resources.Load(tile, typeof(GameObject)), newTilePos, Quaternion.identity);
            newTile.GetComponent<Tile>().targetPosition = newTilePos;
            newTile.GetComponent<Tile>().tilePoints = tileToEvolveValue;
            newTile.GetComponent<Tile>().tileValue.text = tileToEvolveValue.ToString();
            newTile.GetComponent<Tile>().SetColor(tileToEvolveValue);

            newTile.GetComponent<Tile>().evolvedTile = true;

            UIManager.Instance.ActualPoints = UpdateScore(tileToEvolveValue);

            //ScoreText.text = Utils.UpdateScore(tileToEvolveValue);
            UpdateTilesList();
        }
        catch (IndexOutOfRangeException ex)
        {
            Debug.Log("An Error Of Overlay Tile Occurred During Evolution Of Tiles : " + ex);
            OutOfRangeErrorFix();
        }
    }

    public bool LuckyTile()
    {
        float chanceOfCoin = UnityEngine.Random.Range(0, 100);
        if (chanceOfCoin < ControlManager.Instance.CoinChance)
            return true;
        else
            return false;
    }

    void GenerateNewTile()
    {
        float newTileValue = 2f;
        // spawn new tile at random position
        Vector3 locationForNewTile = GameControlManager.Instance.GetRandomLocationForNewTile();
        string tile = ControlManager.Instance.TileSelect;
        float chanceOfTwo = UnityEngine.Random.Range(0, 100);
        if (chanceOfTwo > 91f)
        {
            tile = ControlManager.Instance.TileSelect;
            newTileValue = 4f;
        }
        GameObject newTile = (GameObject)Instantiate(Resources.Load(tile, typeof(GameObject)), locationForNewTile, Quaternion.identity);
        newTile.GetComponent<Tile>().targetPosition = locationForNewTile;
        newTile.GetComponent<Tile>().tilePoints = newTileValue;
        newTile.GetComponent<Tile>().tileValue.text = newTileValue.ToString();
        newTile.GetComponent<Tile>().SetColor(newTileValue);

        newTile.GetComponent<Tile>().coinTile = false;
        newTile.GetComponent<Tile>().getCoin.SetActive(false);

        GameControlManager.Instance.UpdateTilesList();
        GameControlManager.Instance.UpdateGrid();
        GameControlManager.Instance.CountTiles();
    }
}