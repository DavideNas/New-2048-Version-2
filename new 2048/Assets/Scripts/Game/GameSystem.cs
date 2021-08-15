using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance { get; private set; }

    private int newMove;
    public int NewMove{ 
        get { return newMove; }
        set { newMove = value; }    
    }

    private void Start()
    {
        if (null == Instance)
            Instance = this;
    }

    public bool LuckyTile()
    {
        float chanceOfCoin = UnityEngine.Random.Range(0, 100);
        if (chanceOfCoin < ControlManager.Instance.CoinChance)
            return true;
        else
            return false;
    }
}