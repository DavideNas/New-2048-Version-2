using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public static ControlManager Instance { get; private set; }

    // Depth of tiles in Z axis
    public static float depthTile = 5f;
    
    // Size of grid
    private int gridSize = 4;

    // # of new tiles spawned each turn
    private int newTilesPerMove = 1;

    // status sound
    public bool SoundFx = true;

    // Bool check for new game or not
    private bool newGame = true;

    public float DepthTile {
        get { return depthTile; }
        set { depthTile = value; }
    }

    public int GridSize {
        get { return gridSize; }
        set { gridSize = value; }
    }

    public int NewTilesPerMove {
        get { return newTilesPerMove; }
        set { newTilesPerMove = value; }
    }

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