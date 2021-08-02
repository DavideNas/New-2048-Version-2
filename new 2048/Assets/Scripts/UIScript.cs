using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    public static void Grid3x3()
    {
        StageSetting.Instance.GridSize = 3;
        StageSetting.Instance.NewTilesPerMove = 1;
        if (/*SaveSystem.DataExists()*/false)
        {
            //Instance.PopupContinue.SetActive(true);
            //SoundFX.Instance.AlertFX();
        }
        else
        {
            StageSetting.Instance.NewGame = true;
            //Utils.coinChance = 0f;
            SceneManager.LoadScene("Game");
        }
    }

    public static void Grid4x4()
    {
        StageSetting.Instance.GridSize = 4;
        StageSetting.Instance.NewTilesPerMove = 1;
        if (/*SaveSystem.DataExists()*/false)
        {
            //Instance.PopupContinue.SetActive(true);
            //SoundFX.Instance.AlertFX();
        }
        else
        {
            StageSetting.Instance.NewGame = true;
            //Utils.coinChance = 0f;
            SceneManager.LoadScene("Game");
        }
    }

    public static void Grid5x5()
    {
        StageSetting.Instance.GridSize = 5;
        StageSetting.Instance.NewTilesPerMove = 2;
        if (/*SaveSystem.DataExists()*/false)
        {
            //Instance.PopupContinue.SetActive(true);
            //SoundFX.Instance.AlertFX();
        }
        else
        {
            StageSetting.Instance.NewGame = true;
            //Utils.coinChance = 0f;
            SceneManager.LoadScene("Game");
        }
    }

    public static void Grid6x6()
    {
        StageSetting.Instance.GridSize = 6;
        StageSetting.Instance.NewTilesPerMove = 3;
        if (/*SaveSystem.DataExists()*/false)
        {
            //Instance.PopupContinue.SetActive(true);
            //SoundFX.Instance.AlertFX();
        }
        else
        {
            StageSetting.Instance.NewGame = true;
            //Utils.coinChance = 0f;
            SceneManager.LoadScene("Game");
        }
    }

    public static void Grid8x8()
    {
        StageSetting.Instance.GridSize = 8;
        StageSetting.Instance.NewTilesPerMove = 4;
        if (/*SaveSystem.DataExists()*/false)
        {
            //Instance.PopupContinue.SetActive(true);
            //SoundFX.Instance.AlertFX();
        }
        else
        {
            StageSetting.Instance.NewGame = true;
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
