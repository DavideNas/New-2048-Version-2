﻿using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System;

using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    private float score;
    public float Score {
        get{ return score; }
        set{ score = value; }
    }

    private string SAVE_FOLDER;

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
        SAVE_FOLDER = Application.persistentDataPath + "/Saves/";
    }

    public void Init() 
    {
        // Test if Save Folder exists
        if (!Directory.Exists(SAVE_FOLDER)) {
            // Create Save Foldera
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public void Save(string data)
    {
        // create a formatter var to

        BinaryFormatter formatter = new BinaryFormatter();

        string path = SAVE_FOLDER + "save_point.fun";

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public string Load()
    {
        string path = SAVE_FOLDER + "save_point.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            string data = formatter.Deserialize(stream) as string;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }

    public bool DataExists()
    {
        // Check if any data file already exist
        string path = SAVE_FOLDER + "save_point.fun";
        if (File.Exists(path))
            return true;
        else
            return false;
    }

    public void SaveState()
    {
        GameControlManager.Instance.allTilesValue.Clear();
        GameControlManager.Instance.allTilesPositions.Clear();

        GameControlManager.Instance.CheckTilesInfo();                       // read and save all data info (value & position) abput tiles

        SaveObject saveObject = new SaveObject                              // Save an istant of Stage 
        {
            totalScore_level = (int)Score,                                  // 1- score
            tileType_level = ControlManager.Instance.TileSelect,            // 2- type of tyles
            gridSize_level = ControlManager.Instance.GridSize,              // 3- gridsize
            tileToSpawnNo_level = ControlManager.Instance.NewTilesPerMove,  // 4- # tile to spawn each turn
            tilesOnGrid_level = new List<float>(GameControlManager.Instance.allTilesValue),
                                                                            // 5- level of any tile on stage
            tilesPositions_level = new List<Vector3>(GameControlManager.Instance.allTilesPositions),
                                                                            // 6- position (x,y,z) of any tile
            continueOpt = ControlManager.Instance.ContinueCount,            // 7- continue option
        };
        string json = JsonUtility.ToJson(saveObject);
        Instance.Save(json);

        //UnityEngine.Debug.Log("State saved ! -> "+GameControlManager.Instance.allTilesValue);
    }

    public void LoadState()                                                 // Load data of previous game
    {
        SaveObject data = new SaveObject();
        string saveString = Instance.Load();
        JsonUtility.FromJsonOverwrite(saveString, data);
        Score = (int)data.totalScore_level;                                 // 1- score
        ControlManager.Instance.TileSelect = (string)data.tileType_level;                           
                                                                            // 2- type of tyles
        ControlManager.Instance.GridSize = (int)data.gridSize_level;        // 3- gridsize
        ControlManager.Instance.NewTilesPerMove = (int)data.tileToSpawnNo_level;
                                                                            // 4- # tile to spawn each turn
        GameControlManager.Instance.allTilesValue = new List<float>(data.tilesOnGrid_level);
                                                                            // 5- level of any tile on stage
        GameControlManager.Instance.allTilesPositions = new List<Vector3>(data.tilesPositions_level);
                                                                            // 6- position (x,y,z) of any tile
        ControlManager.Instance.ContinueCount = data.continueOpt;           // 7- continue option

        //UnityEngine.Debug.Log("State loaded ! GridSize: "+ ControlManager.Instance.GridSize);
    }

    private class SaveObject                        // data structure for Save/Load stage info
    {
        public int totalScore_level;                // 1- score
        public string tileType_level;               // 2- type of tyles
        public int gridSize_level;                  // 3- gridsize
        public int tileToSpawnNo_level;             // 4- # tile to spawn each turn
        public List<float> tilesOnGrid_level;       // 5- level of any tile on stage
        public List<Vector3> tilesPositions_level;  // 6- position (x,y,z) of any tile
        public int continueOpt;                     // 7- continue option
    }
}