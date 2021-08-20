using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    //
    public static void SaveState()
    {
        GameControlManager.Instance.allTilesValue.Clear();
        GameControlManager.Instance.allTilesPositions.Clear();

        GameControlManager.Instance.CheckTilesInfo();                       // read and save all data info (value & position) abput tiles
        
        SaveObject saveObject = new SaveObject                              // Save an istant of Stage 
        {
            totalScore_level = (int)ControlManager.Instance.Score,          // 1- score
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
        SaveSystem.Save(json);

        //SSTools.ShowMessage("Button Home Pressed", SSTools.Position.bottom, SSTools.Time.threeSecond);
    }

    //
    public static void LoadState()                                          // Load data of previous game
    {
        SaveObject data = new SaveObject();
        string saveString = SaveSystem.Load();
        JsonUtility.FromJsonOverwrite(saveString, data);
        ControlManager.Instance.Score = (int)data.totalScore_level;         // 1- score
        ControlManager.Instance.TileSelect = (string)data.tileType_level;   // 2- type of tyles
        ControlManager.Instance.GridSize = (int)data.gridSize_level;        // 3- gridsize
        ControlManager.Instance.NewTilesPerMove = (int)data.tileToSpawnNo_level;
                                                                            // 4- # tile to spawn each turn
        GameControlManager.Instance.allTilesValue = new List<float>(data.tilesOnGrid_level);
                                                                            // 5- level of any tile on stage
        GameControlManager.Instance.allTilesPositions = new List<Vector3>(data.tilesPositions_level);
                                                                            // 6- position (x,y,z) of any tile
        ControlManager.Instance.ContinueCount = data.continueOpt;           // 7- continue option

        //SSTools.ShowMessage("Try to load Previous stage", SSTools.Position.bottom, SSTools.Time.threeSecond);
        //UnityEngine.Debug.Log("State loaded ! GridSize: "+ ControlManager.Instance.GridSize);
    }

    //
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
