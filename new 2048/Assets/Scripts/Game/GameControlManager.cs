using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GameControlManager : MonoBehaviour
{
    public static GameControlManager Instance { get; private set; }

    public Swipe swipeControls;

    private bool prevScoreSaved;
    public bool PrevScoreSaved{ 
        get { return prevScoreSaved; }
        set { prevScoreSaved = value; }
    }

    private float velocity;
    public float Velocity{
        get { return velocity; }
        set { velocity = value; }
    }

    /*private bool undoOk;
    public bool UndoOk{
        get{ return undoOk; }
        set { undoOk = value; }
    }*/

    private int countPossibleMove;
    private int CountPossibleMove{
        get { return countPossibleMove; }
        set { countPossibleMove = value; }
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

    // boolean var to check if tiles are in movement
    private bool allTilesAreSteady;
    public bool AllTilesAreSteady{
        get { return allTilesAreSteady; }
        set { allTilesAreSteady = value; }
    }

    private bool tilePosAreChanged;
    private bool TilePosAreChanged{
        get{ return tilePosAreChanged; }
        set{ tilePosAreChanged = value; }
    }

    private float sizeScreen;
    private float SizeScreen{
        get { return sizeScreen; }
        set { sizeScreen = value; }
    }

    // contains all free position of the grid
    private List<Vector2> voidOnGrid;

    // 
    private int moveCount;
    public int MoveCount{
        get { return moveCount; }
        set { moveCount = value; }
    }               

    // previous points of game stage
    private float prevScore;

    public List<float> allScoreStack;

    // info for all tiles on stage
    public List<Vector3> allTilesPositions;
    public List<float> allTilesValue;

    // info for Undo check
    public List<Vector3> allTilesPositionsUndo;
    public List<float> allTilesValueUndo;

    public List<List<float>> allTilesValueStack;
    public List<List<Vector3>> allTilesPositionsStack;

    // list to manage tiles on stage
    private List<GameObject> tilesOnGrid;

    // lists to manipulate rows and columns
    public List<GameObject>[] rowGrid;
    public List<GameObject>[] colGrid;

    private void Start()
    {
        GC.Collect();

        if (null == Instance)
            Instance = this;

        PrevScoreSaved = false;

        SaveSystem.Instance.Score = 0f;
        prevScore = 0f;

        MoveCount = 0;

        Velocity = 20f;

        ControlManager.Instance.ContinueCount = 0;

        AllTilesAreSteady = false;

        SizeScreen = Screen.width;

        TilePosAreChanged = false;

        //UndoOk = false;

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

        if(ControlManager.Instance.NewGame)
        {
            LevelSetup.Instance.InitGrid(); 
            for (int x = 0; x < ControlManager.Instance.StartTilesNo; x++)
            {
                Invoke("GenerateNewTile", ControlManager.Instance.SpawnDelayTime + (x * (ControlManager.Instance.SpawnDelayTime / ControlManager.Instance.StartTilesNo)));
            }
        }
        else
        {
            //Debug.Log("Load Previous Data !");
            SaveSystem.Instance.LoadState();
            LevelSetup.Instance.InitGrid();                         // Create base grid
            LevelSetup.Instance.RestoreStage();

            //UIManager.Instance.ActualPoints = SaveSystem.Instance.Score.ToString();

            /*CheckFreePos();
            UpdateTilesList();
            UpdateGrid();
            CountTiles();*/
        }
    }

    void Update()
    {
        if (SizeScreen != Screen.width)
        {
            SizeScreen = Screen.width;
            LevelSetup.Instance.SetCameraSize();
        }

        //CoinsText.text = Utils.coins.ToString();

        CheckUserInput();
        try
        {
            if (AllTilesAreSteady)
            {
                TilePosAreChanged = true;
                for (int n = 0; n < ControlManager.Instance.NewTilesPerMove; n++)
                {
                    Invoke("GenerateNewTile", ControlManager.Instance.SpawnDelayTime + (n * (ControlManager.Instance.SpawnDelayTime / ControlManager.Instance.NewTilesPerMove)));
                                                // time elapsed between various new spawn to make game more fluid (some lag if they're spawned togheter)
                }
                AllTilesAreSteady = false;
            }
        }
        catch (IndexOutOfRangeException ex)
        {
            Debug.Log("An Error Of Overlay Tile Occurred During Update: " + ex);
            OutOfRangeErrorFix();
        }
    }

    void OnApplicationPause()
    {
        SaveSystem.Instance.SaveState();
    }

    void OnApplicationQuit()
    {
        SaveSystem.Instance.SaveState();
    }

    private void CheckUserInput()
    {
        try
        {
            // if all tiles are stationary
            if (swipeControls.SwipeLeft && !ZeroCase() && !GameOver())
            {
                if ((!TilesAreMoving()) && (MoveCount == NewMove))
                {
                    Debug.Log("MoveCount -> "+MoveCount+", NewMove -> "+NewMove);
                    StartCoroutine(CheckNextInput());
                    MoveAllTiles(Vector3.left);
                }
            }
            else if (swipeControls.SwipeRight && !ZeroCase() && !GameOver())
            {
                if (!TilesAreMoving() && (MoveCount == NewMove))
                {
                    StartCoroutine(CheckNextInput());
                    MoveAllTiles(Vector3.right);
                }
            }
            else if (swipeControls.SwipeUp && !ZeroCase() && !GameOver())
            {
                if (!TilesAreMoving() && (MoveCount == NewMove))
                {
                    StartCoroutine(CheckNextInput());
                    MoveAllTiles(Vector3.up);
                }
            }
            else if (swipeControls.SwipeDown && !ZeroCase() && !GameOver())
            {
                if (!TilesAreMoving() && (MoveCount == NewMove))
                {
                    StartCoroutine(CheckNextInput());
                    MoveAllTiles(Vector3.down);
                }
            }
        }
        catch (IndexOutOfRangeException ex)
        {
            Debug.Log("An Error Of Overlay Tile Occurred During Check User Input : " + ex);
            OutOfRangeErrorFix();
        }
    }

    bool ZeroCase()
    {
        CountTiles();
        if (tilesOnGrid.Count == 0)
        {
            GenerateNewTile();
            return true;
        }
        else
            return false;
    }

    bool GameOver()
    {
        GC.Collect();
        CountPossibleMove = 0;

        if (ControlManager.Instance.ContinueCount > 0 )
        {
            UIManager.Instance.ContinueOption.SetActive(false);
        }else
        {
            UIManager.Instance.ContinueOption.SetActive(true);
        }

        // check if there are other possible movement to stage

        if (tilesOnGrid.Count < ControlManager.Instance.GridSize * ControlManager.Instance.GridSize)
        {
            CountPossibleMove++;
        }
        else
        {
            UpdateTilesList();

            // check every line in rowGrid
            for (int i = 0; i < rowGrid.Length; i++)
            {
                // scan every single element in the line
                for (int j = 0; j < ControlManager.Instance.GridSize - 1; j++)
                {
                    string selectedTile = rowGrid[i][j].GetComponent<Tile>().tileValue.text;
                    string selectedNextTile;
                    if (null != rowGrid[i][j + 1].GetComponent<Tile>().tileValue.text)
                    {
                        selectedNextTile = rowGrid[i][j + 1].GetComponent<Tile>().tileValue.text;
                    }
                    else
                    {
                        selectedNextTile = "0";
                    }
                    // compare one element in the line with his next one
                    if (Equals(selectedTile, selectedNextTile))
                    {
                        CountPossibleMove++;
                    }
                }
            }

            // check every line in colGrid
            for (int k = 0; k < colGrid.Length; k++)
            {
                // scan every single element in the line
                for (int t = 0; t < ControlManager.Instance.GridSize - 1; t++)
                {
                    string selectedTile = colGrid[k][t].GetComponent<Tile>().tileValue.text;
                    string selectedNextTile = colGrid[k][t + 1].GetComponent<Tile>().tileValue.text;
                    // compare one element in the line with his next one
                    if (Equals(selectedTile, selectedNextTile))
                    {
                        CountPossibleMove++;
                    }
                }
            }
        }

        // if there are not other possibilities
        if (CountPossibleMove > 0)
        {
            // continue to play
            // Debug.Log("Found " + countPossibleMove + "possible matches");
            return false;
        }
        else
        {
            // else open game over popup
            SoundFX.Instance.AlertFX();
            UIManager.Instance.GameOver.SetActive(true);
            return true;
        }
    }

    IEnumerator CheckNextInput()
    {
        MoveCount++;

        //Print the time of when the function is first called.

        yield return new WaitForSeconds(0.8f);

        UndoReady = true;

        if (TilePosAreChanged)
        {
            NewMove++;
            TilePosAreChanged = false;                      // reset control variable
            SaveTileStack();
        }
        else
        {
            MoveCount--;
        }

        CheckFreePos();

        //After we have waited 5 seconds print the time again.
        //UnityEngine.Debug.Log("Now player can move again : moveTot -> " + Utils.moveCount);
    }

    bool TilesAreMoving()
    {
        // make a control for every tile on stage
        // and return false if all animation are finished (tiles are all stationary)
        int checkSlide = 0;
        foreach (GameObject tileInMotion in tilesOnGrid)
        {
            if (tileInMotion.GetComponent<Tile>().isAnimate)
            {
                checkSlide++;    
            }
        }

        if (checkSlide == 0)
            return false;       // if all tiles are stationary
        else
            return true;        // if any tile is moving
        
        // create a temp var that will permit new movement on stage after a specific time (i.e. max 1 move every 0.5 seconds)
    }

    void GenerateNewTile()
    {
        float newTileValue = 2f;
        // spawn new tile at random position
        Vector3 locationForNewTile = GetRandomLocationForNewTile();
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
        //newTile.GetComponent<Tile>().SetColor(newTileValue);

        //newTile.GetComponent<Tile>().coinTile = false;
        //newTile.GetComponent<Tile>().getCoin.SetActive(false);

        UpdateTilesList();
        UpdateGrid();
        CountTiles();
        //Debug.Log("New tile on grid at : "+locationForNewTile);
    }

    // update points on stage with zeros
    public string UpdateScore(float points)
    {
        if (!PrevScoreSaved)
        {
            prevScore = SaveSystem.Instance.Score;
            PrevScoreSaved = true;
            allScoreStack.Clear();
        }

        string scoreUpdated = "";
        SaveSystem.Instance.Score += points / 2;

        allScoreStack.Add(SaveSystem.Instance.Score);                               // save score into list for Undo recover

        scoreUpdated += SaveSystem.Instance.Score.ToString();

        float recordcheck = float.Parse(UIManager.Instance.scoreRecord.text);
        if (Math.Abs(recordcheck) <= Math.Abs(SaveSystem.Instance.Score))
        {
            long newRecord = Convert.ToInt64(SaveSystem.Instance.Score);

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

            UIManager.Instance.scoreRecord.text = SaveSystem.Instance.Score.ToString();
        }
        
        return scoreUpdated;
    }

    // function to read and save all tiles info in the stage
    public void CheckTilesInfo()
    {
        allTilesPositions.Clear();
        allTilesValue.Clear();
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
        SaveSystem.Instance.Score = prevScore;

        if (allTilesValueStack.Count > 1)
        {
            Debug.Log("Load previous stage !");
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

                //loadedTile.GetComponent<Tile>().SetColor(allTilesValueUndo[t]);
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
        SaveSystem.Instance.Score = 0f;
        // Clean all stage from Tiles
        foreach (GameObject singleTile in tilesOnGrid)          
        {
            if (singleTile != null)
            {
                Destroy(singleTile.gameObject);
            }
        }
    }
    
    void UpdateTilesList()
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

    // Convert coord in grid position - Y line
    public int Find_Y(GameObject tileObject)
    {
        int yFound = -1;
        for (int x = 0; x < ControlManager.Instance.GridSize; x++)
        {
            for (int y = 0; y < ControlManager.Instance.GridSize; y++)
            {
                if ((tileObject.transform.position.x == LevelSetup.Instance.gridCoord[x, y].x) && (tileObject.transform.position.y == LevelSetup.Instance.gridCoord[x, y].y))
                    yFound = y;
            }
        }
        return yFound;
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
                if ((tileObject.transform.position.x == LevelSetup.Instance.gridCoord[x, y].x) && (tileObject.transform.position.y == LevelSetup.Instance.gridCoord[x, y].y))
                    xFound = x;
            }
        }
        return xFound;
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
            ResetMaps();
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

    void ResetMaps()
    {
        for(int k=0; k < ControlManager.Instance.GridSize;k++)
        {
            rowGrid[k] = new List<GameObject>();
            colGrid[k] = new List<GameObject>();
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
            foreach (Vector2 coordToCompare in LevelSetup.Instance.gridCoord)
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
    Vector3 GetRandomLocationForNewTile()
    {
        //try 
        //{
        CountTiles();
        CheckFreePos();

        //if (0 < voidOnGrid.Count) 
        //{
        // *estraggo una cella libera dalla griglia
        int randIndex = UnityEngine.Random.Range(0, voidOnGrid.Count);

        // *coordinata x della cella libera
        randX = voidOnGrid[randIndex].x;

        // *coordinata y della cella libera
        randY = voidOnGrid[randIndex].y;

        return new Vector3(randX, randY, ControlManager.Instance.DepthTile);
        //} else
        //{
        //    return new Vector3(0,0,0);
        //}
        /*}
        catch (IndexOutOfRangeException ex)
        {
            Debug.Log("An Error Of Overlay Tile Occurred During Evolution Of Tiles : " + ex);
            OutOfRangeErrorFix();
        }*/
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
            //newTile.GetComponent<Tile>().SetColor(tileToEvolveValue);

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

    void MoveAllTiles(Vector3 direction)
    {
        try
        {
            // prevuois score saving check var (used at start of movement)
            PrevScoreSaved = false;

            //axis = MoveAxis(direction);

            UpdateTilesList();

            foreach (GameObject myTile in tilesOnGrid)
            {
                float posX = myTile.transform.position.x;
                float posY = myTile.transform.position.y;
                int cellSpace = 0;
                OrderRowAndGrid();
                if (direction.x != 0)
                {
                    posX = AssignNewX(myTile, direction.x);
                    myTile.GetComponent<Tile>().targetPosition = new Vector3(posX, posY, ControlManager.Instance.DepthTile);
                    int oldCoord = Find_X(myTile);
                    int newCoord = 0;
                    for (int p = 0; p < ControlManager.Instance.GridSize; p++)
                    {
                        if (posX == LevelSetup.Instance.lineCoord[p])
                            newCoord = p;
                    }
                    if (oldCoord < newCoord)
                    {
                        cellSpace = newCoord - oldCoord;
                        AllTilesAreSteady = true;
                    }
                    else if (oldCoord > newCoord)
                    {
                        cellSpace = oldCoord - newCoord;
                        AllTilesAreSteady = true;
                    }
                }
                else if (direction.y != 0)
                {
                    posY = AssignNewY(myTile, direction.y);
                    myTile.GetComponent<Tile>().targetPosition = new Vector3(posX, posY, ControlManager.Instance.DepthTile);
                    int oldCoord = Find_Y(myTile);
                    int newCoord = 0;
                    for (int p = 0; p < ControlManager.Instance.GridSize; p++)
                    {
                        if (posY == LevelSetup.Instance.lineCoord[p])
                            newCoord = p;
                    }
                    if (oldCoord < newCoord)
                    {
                        cellSpace = newCoord - oldCoord;
                        AllTilesAreSteady = true;
                    }
                    else if (oldCoord > newCoord)
                    {
                        cellSpace = oldCoord - newCoord;
                        AllTilesAreSteady = true;
                    }
                }
                myTile.GetComponent<Tile>().speed = cellSpace;
            }
            AdjustForMatching(direction);
        }
        catch (IndexOutOfRangeException ex)
        {
            Debug.Log("An Error Of Overlay Tile Occurred During Move Of Tiles Function: " + ex);
            OutOfRangeErrorFix();
        }
    }

    //    float AssignNewCoord(GameObject thisTile, Vector3 whereDirection)
    float AssignNewX(GameObject thisTile, float whereDirection)
    {
        float newCoordX = thisTile.transform.position.x;

        try
        {
            int thisTileY = Find_Y(thisTile);

            // if go left
            if (whereDirection == -1)
            {
                for (int i = 0; i < rowGrid[thisTileY].Count; i++)
                {
                    if (thisTile.transform.position.x == rowGrid[thisTileY][i].transform.position.x)
                    {
                        float fromLeft = LevelSetup.Instance.lineCoord[i];
                        newCoordX = fromLeft;
                    }
                }
                // assign to every tile new coord x from left to right 
                // * the order of x can be found by reading gridCoord .x, from first to last
            }
            // if go right
            else if (whereDirection == 1)
            {
                for (int i = 0; i < rowGrid[thisTileY].Count; i++)
                {
                    if (thisTile.transform.position.x == rowGrid[thisTileY][i].transform.position.x)
                    {
                        int fromEnd = ControlManager.Instance.GridSize - (rowGrid[thisTileY].Count - i);
                        float fromRight = LevelSetup.Instance.lineCoord[fromEnd];
                        newCoordX = fromRight;
                    }
                }

                // assign to every tile new coord x from right to left 
                // * the order of x can be found by reading gridCoord .x, from last to first
            }

            //return newCoordX;
        }
        catch (IndexOutOfRangeException ex)
        {
            Debug.Log("An Error Occur During Assign New X Function : " + ex);
            OutOfRangeErrorFix();
        }
        return newCoordX;
    }

    float AssignNewY(GameObject thisTile, float whereDirection)
    {
        float newCoordY = thisTile.transform.position.y;
        try
        {
            int thisTileX = Find_X(thisTile);

            // if go up
            if (whereDirection == 1)
            {
                for (int i = 0; i < colGrid[thisTileX].Count; i++)
                {
                    if (thisTile.transform.position.y == colGrid[thisTileX][i].transform.position.y)
                    {
                        int fromEnd = ControlManager.Instance.GridSize - (colGrid[thisTileX].Count - i);
                        float fromTop = LevelSetup.Instance.lineCoord[fromEnd];
                        newCoordY = fromTop;
                    }
                }
                // assign to every tile new coord y from bottom to top
                // * the order of y can be found by reading gridCoord .y, from first to last
            }
            // if go down
            else if (whereDirection == -1)
            {
                for (int i = 0; i < colGrid[thisTileX].Count; i++)
                {
                    if (thisTile.transform.position.y == colGrid[thisTileX][i].transform.position.y)
                    {
                        float fromBottom = LevelSetup.Instance.lineCoord[i];
                        newCoordY = fromBottom;
                    }
                }

                // assign to every tile new coord y from top to bottom *
                // * the order of y can be found by reading gridCoord .y, from last to first 
            }            
        }
        catch (IndexOutOfRangeException ex)
        {
            Debug.Log("An Error Occur During Assign New Y Function : " + ex);
            OutOfRangeErrorFix();
        }
        return newCoordY;
    }

    void AdjustForMatching(Vector3 directTo)
    {
        try
        {
            bool isMatching;
            int matchCounter;

            //Se ho spostato a sinistra
            if (directTo.x == -1)
            {
                // Scandisco riga per riga
                for (int y = 0; y < ControlManager.Instance.GridSize; y++)
                {
                    matchCounter = 0;
                    isMatching = false;
                    // vado avanti se la riga non è vuota
                    if (rowGrid[y].Count > 0)
                    {
                        // Leggo le tessere da sinistra -> a destra
                        for (int k = 0; k < rowGrid[y].Count; k++)
                        {
                            int differenziale = 0;
                            //salvo coordinate per il confronto
                            float posX = rowGrid[y][k].transform.position.x;
                            float posY = rowGrid[y][k].transform.position.y;

                            float thisTilePoints = rowGrid[y][k].GetComponent<Tile>().tilePoints;
                            float nextTilePoints;
                            // Se esiste una tessera che sussegue sulla destra rispetto quella considerata
                            int nextPointer = k + 1;
                            if (nextPointer < rowGrid[y].Count)
                                // Salvo il punteggio per confrontarlo con quello attuale
                                nextTilePoints = rowGrid[y][nextPointer].GetComponent<Tile>().tilePoints;
                            else
                                nextTilePoints = 0f;

                            differenziale = k - matchCounter;
                            posX = LevelSetup.Instance.lineCoord[differenziale];
                            rowGrid[y][k].GetComponent<Tile>().targetPosition = new Vector3(posX, posY, ControlManager.Instance.DepthTile);
                            rowGrid[y][k].GetComponent<Tile>().speed += matchCounter;

                            if (!isMatching)
                            {
                                if (thisTilePoints == nextTilePoints)
                                {
                                    matchCounter++;
                                    isMatching = true;
                                    //Debug.Log("Some match for LEFT direction");
                                    SoundFX.Instance.MatchTilesFX();
                                }
                            }
                            else
                            {
                                AllTilesAreSteady = true;
                                isMatching = false;
                                //Debug.Log("There are no match for LEFT move");
                            }
                        }
                    }
                }
            }
            //Se ho spostato a destra
            else if (directTo.x == 1)
            {
                // Scandisco riga per riga
                for (int y = 0; y < ControlManager.Instance.GridSize; y++)
                {
                    matchCounter = 0;
                    isMatching = false;
                    // vado avanti se la riga non è vuota
                    if (rowGrid[y].Count > 0)
                    {
                        // Leggo le tessere da destra -> a sinistra
                        for (int k = rowGrid[y].Count - 1; k > -1; k--)
                        {
                            int differenziale = 0;
                            //salvo coordinate per il confronto
                            float posX = rowGrid[y][k].transform.position.x;
                            float posY = rowGrid[y][k].transform.position.y;

                            float thisTilePoints = rowGrid[y][k].GetComponent<Tile>().tilePoints;
                            float prevTilePoints;
                            // Se esiste una tessera che precede sulla sinistra rispetto quella considerata
                            int prevPointer = k - 1;
                            if (prevPointer >= 0)
                                // Salvo il punteggio per confrontarlo con quello attuale
                                prevTilePoints = rowGrid[y][prevPointer].GetComponent<Tile>().tilePoints;
                            else
                                prevTilePoints = 0f;

                            differenziale = ControlManager.Instance.GridSize - (rowGrid[y].Count - (k + matchCounter));
                            posX = LevelSetup.Instance.lineCoord[differenziale];
                            rowGrid[y][k].GetComponent<Tile>().targetPosition = new Vector3(posX, posY, ControlManager.Instance.DepthTile);
                            rowGrid[y][k].GetComponent<Tile>().speed += matchCounter;

                            if (!isMatching)
                            {
                                if (thisTilePoints == prevTilePoints)
                                {
                                    matchCounter++;
                                    isMatching = true;
                                    //Debug.Log("Some match for RIGHT direction");
                                    SoundFX.Instance.MatchTilesFX();
                                }
                            }
                            else
                            {
                                AllTilesAreSteady = true;
                                isMatching = false;
                                //Debug.Log("There are no match for RIGHT move");
                            }
                        }
                    }
                }
            }
            //Se ho spostato in giù
            else if (directTo.y == -1)
            {
                // Scandisco colonna per colonna
                for (int x = 0; x < ControlManager.Instance.GridSize; x++)
                {
                    matchCounter = 0;
                    isMatching = false;
                    // vado avanti se la colonna non è vuota
                    if (colGrid[x].Count > 0)
                    {
                        // Leggo le tessere dal basso -> all'alto 
                        for (int k = 0; k < colGrid[x].Count; k++)
                        {
                            int differenziale = 0;
                            //salvo coordinate per il confronto
                            float posX = colGrid[x][k].transform.position.x;
                            float posY = colGrid[x][k].transform.position.y;

                            float thisTilePoints = colGrid[x][k].GetComponent<Tile>().tilePoints;
                            float nextTilePoints;
                            // Se esiste una tessera che sussegue verso l'alto rispetto a quella considerata
                            int nextPointer = k + 1;
                            if (nextPointer < colGrid[x].Count)
                                // Salvo il punteggio per confrontarlo con quello attuale
                                nextTilePoints = colGrid[x][nextPointer].GetComponent<Tile>().tilePoints;
                            else
                                nextTilePoints = 0f;

                            differenziale = k - matchCounter;
                            posY = LevelSetup.Instance.lineCoord[differenziale];
                            colGrid[x][k].GetComponent<Tile>().targetPosition = new Vector3(posX, posY, ControlManager.Instance.DepthTile);
                            colGrid[x][k].GetComponent<Tile>().speed += matchCounter;

                            if (!isMatching)
                            {
                                if (thisTilePoints == nextTilePoints)
                                {
                                    matchCounter++;
                                    isMatching = true;
                                    //Debug.Log("Some match for DOWN direction");
                                    SoundFX.Instance.MatchTilesFX();
                                }
                            }
                            else
                            {
                                AllTilesAreSteady = true;
                                isMatching = false;
                                //Debug.Log("There are no match for UP move");
                            }
                        }
                    }
                }
            }
            //Se ho spostato in su
            else if (directTo.y == 1)
            {
                // Scandisco colonna per colonna
                for (int x = 0; x < ControlManager.Instance.GridSize; x++)
                {
                    matchCounter = 0;
                    isMatching = false;
                    // vado avanti se la colonna non è vuota
                    if (colGrid[x].Count > 0)
                    {
                        // Leggo le tessere dall'alto -> al basso
                        for (int k = colGrid[x].Count - 1; k > -1; k--)
                        {
                            int differenziale = 0;
                            //salvo coordinate per il confronto
                            float posX = colGrid[x][k].transform.position.x;
                            float posY = colGrid[x][k].transform.position.y;

                            float thisTilePoints = colGrid[x][k].GetComponent<Tile>().tilePoints;
                            float prevTilePoints;
                            // Se esiste una tessera che precede dal basso rispetto a quella considerata
                            int prevPointer = k - 1;
                            if (prevPointer >= 0)
                            {
                                // Salvo il punteggio per confrontarlo con quello attuale
                                prevTilePoints = colGrid[x][prevPointer].GetComponent<Tile>().tilePoints;
                            }
                            else
                                prevTilePoints = 0f;

                            differenziale = ControlManager.Instance.GridSize - (colGrid[x].Count - (k + matchCounter));
                            posY = LevelSetup.Instance.lineCoord[differenziale];
                            colGrid[x][k].GetComponent<Tile>().targetPosition = new Vector3(posX, posY, ControlManager.Instance.DepthTile);
                            colGrid[x][k].GetComponent<Tile>().speed += matchCounter;

                            if (!isMatching)
                            {
                                if (thisTilePoints == prevTilePoints)
                                {
                                    matchCounter++;
                                    isMatching = true;
                                    //Debug.Log("Some match for UP direction");
                                    SoundFX.Instance.MatchTilesFX();
                                }
                            }
                            else
                            {
                                AllTilesAreSteady = true;
                                isMatching = false;
                                //Debug.Log("There are no match for DOWN move");
                            }
                        }
                    }
                }
            }
        }
        catch (IndexOutOfRangeException ex)
        {
            Debug.Log("An Error Occur During Adjust Function : " + ex);
            OutOfRangeErrorFix();
        }
    }
    /*char MoveAxis(Vector3 inAxis)
    {
        if (inAxis.x != 0)
            return 'x';
        else return 'y';
    }*/
}