using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSetting : MonoBehaviour
{
    public static StageSetting Instance { get; private set; }

    private int gridSize = 4; 
    private int newTilesPerMove = 1;
    private bool newGame = true;

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

        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
