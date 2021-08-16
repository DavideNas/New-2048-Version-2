using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public static ControlManager Instance { get; private set; }
    
    // status sound
    public bool SoundFx;

    private float spawnDelayTime;
    public float SpawnDelayTime { 
        get { return spawnDelayTime; }
        set { spawnDelayTime = value; }
    }

    private int startTilesNo;
    public int StartTilesNo { 
        get { return startTilesNo; }
        set { startTilesNo = value; }
    }

    // Theme color of tiles
    private string activeTheme;
    public string ActiveTheme { 
        get{ return activeTheme; }
        set{ activeTheme = value; }
    }

    // magnitude for swipe distance
    private float magnitude;
    public float Magnitude {
        get{ return magnitude; }
        set { magnitude = value; }
    }

    // Depth of tiles in Z axis
    public static float depthTile;
    public float DepthTile {
        get { return depthTile; }
        set { depthTile = value; }
    }

    // Size of grid
    private int gridSize;
    public int GridSize {
        get { return gridSize; }
        set { gridSize = value; }
    }

    // NOT USED
    private float coinChance;
    public float CoinChance { 
        get{ return coinChance; }
        set { coinChance = value; }
    }

    // # of new tiles spawned each turn
    private int newTilesPerMove;    
    public int NewTilesPerMove {
        get { return newTilesPerMove; }
        set { newTilesPerMove = value; }
    }

    // Bool check for new game or not
    private bool newGame;
    public bool NewGame {
        get { return newGame; }
        set { newGame = value; }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    
    private void Start()
    {
        CurrentVersion = Application.version + " - 23 (Main Release)";

        CoinChance = 2f;

        NewTilesPerMove = 1;

        DepthTile = 5f;

        TileSelect = "ClassicTile";
        
        ActiveTheme = "pastel";
        
        Magnitude = 125;

        NewGame = true;

        SoundFx = true;

        StartTilesNo = 2;

        SpawnDelayTime = 0.15f;
    }

    // Tile type
    private string tileSelect;
    public string TileSelect
    {
        get { return tileSelect; }
        set { tileSelect = value; }
    }

    // Version control
    private string currentVersion;
    public string CurrentVersion 
    {
        get { return currentVersion; }
        set { currentVersion = value; }
    }

    // Google connection status
    private bool googlePlayConnection;
    public bool GPlayConnection
    {
        get { return googlePlayConnection; }
        set { googlePlayConnection = value; }
    }
    
    public void UpdateSoundStatus()
    {
        if (Instance.SoundFx)
        {
            UIManager.Instance.SoundOn.SetActive(false);
            UIManager.Instance.SoundOff.SetActive(true);
            Instance.SoundFx = !Instance.SoundFx;
        }
        else
        {
            UIManager.Instance.SoundOn.SetActive(true);
            UIManager.Instance.SoundOff.SetActive(false);
            Instance.SoundFx = !Instance.SoundFx;
        }
    }
    
    public void SwitchConnection(bool connectionState)
    {
        if (connectionState)
        {
            // script to disable connection
            UIManager.Instance.PlayConnected.SetActive(true);
            UIManager.Instance.PlayDisconnected.SetActive(false);
        }
        else
        {
            // script to enable connection
            UIManager.Instance.PlayConnected.SetActive(false);
            UIManager.Instance.PlayDisconnected.SetActive(true);
        }
    }
}