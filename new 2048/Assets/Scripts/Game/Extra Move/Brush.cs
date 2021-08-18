using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Brush : MonoBehaviour
{
    public static Brush Instance { get; private set; }
    //private int brushCost = 0;

    private int tilesFound = 0;

    void Start()
    {
        Instance = this;
    }

    public void BrushTiles() {
        ControlManager.Instance.ContinueCount++;
        //Debug.Log("Brush power !");
        //if ( (Utils.coins - brushCost) > -1 )
        //{
            tilesFound = 0;
            GameObject tempTile;

            GameObject[] allTiles = (GameObject[])FindObjectsOfType(typeof(GameObject));

            for (int k = 0; k < allTiles.Length; k++)
            {
                if (allTiles[k].GetComponent<Tile>())
                {
                    tempTile = allTiles[k];
                    if ((tempTile.GetComponent<Tile>().tilePoints == 2f) || (tempTile.GetComponent<Tile>().tilePoints == 4f))
                    {
                        tilesFound++;
                        Destroy(tempTile.gameObject);
                    }
                }
            }

        //}

        if (tilesFound > 0)
        {
            //Utils.coins -= brushCost;
            Invoke("UpdateTilesOnStage", 0.15f);
        }
    }

    void UpdateTilesOnStage()
    {
        GameControlManager.Instance.SaveTileStack();
        System.GC.Collect();
    }
}