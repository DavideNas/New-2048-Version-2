using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levels : MonoBehaviour
{
    private void Start() {
        ControlManager.Instance.DepthTile = 5;
    }

    public static void Grid3x3()
    {
        ControlManager.Instance.GridSize = 3;
        ControlManager.Instance.NewTilesPerMove = 1;
        if (SaveSystem.DataExists())
        {
            UIManager.Instance.ContinueOption.SetActive(true);
            SoundFX.Instance.AlertFX();
        }
        else
        {

            ControlManager.Instance.NewGame = true;
            //Utils.coinChance = 0f;
            SceneManager.LoadScene("Game");
        }
    }

    public static void Grid4x4()
    {
        ControlManager.Instance.GridSize = 4;
        ControlManager.Instance.NewTilesPerMove = 1;    
        if (SaveSystem.DataExists())
        {
            UIManager.Instance.ContinueOption.SetActive(true);
            SoundFX.Instance.AlertFX();
        }
        else
        {
            ControlManager.Instance.NewGame = true;
            //Utils.coinChance = 0f;
            SceneManager.LoadScene("Game");
        }
    }

    public static void Grid5x5()
    {            
        ControlManager.Instance.GridSize = 5;
        ControlManager.Instance.NewTilesPerMove = 2;
        if (SaveSystem.DataExists())
        {
            UIManager.Instance.ContinueOption.SetActive(true);
            SoundFX.Instance.AlertFX();
        }
        else
        {

            ControlManager.Instance.NewGame = true;
            //Utils.coinChance = 0f;
            SceneManager.LoadScene("Game");
        }
    }

    public static void Grid6x6()
    {
        ControlManager.Instance.GridSize = 6;
        ControlManager.Instance.NewTilesPerMove = 3;
        if (SaveSystem.DataExists())
        {
            UIManager.Instance.ContinueOption.SetActive(true);
            SoundFX.Instance.AlertFX();
        }
        else
        {
            ControlManager.Instance.NewGame = true;
            //Utils.coinChance = 0f;
            SceneManager.LoadScene("Game");
        }
    }

    public static void Grid8x8()
    {
        ControlManager.Instance.GridSize = 8;
        ControlManager.Instance.NewTilesPerMove = 4;
        if (SaveSystem.DataExists())
        {
            UIManager.Instance.ContinueOption.SetActive(true);
            SoundFX.Instance.AlertFX();
        }
        else
        {
            
            ControlManager.Instance.NewGame = true;
            //Utils.coinChance = 0f;
            SceneManager.LoadScene("Game");
        }
    }
    
    public static void BtnLeft()
    {
        SwipeLevels.LeftDirection = true;
        SoundFX.Instance.SelectLevelFX();
    }

    public static void BtnRight()
    {
        SwipeLevels.RightDirection = true;
        SoundFX.Instance.SelectLevelFX();
    }
}
