using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    public GameObject SettingView;

    public GameObject Menu;

    public GameObject GameOver;

    public GameObject ContinueOption;

    public GameObject RestartPopup;

    public Text levelSize;

    public Text version;

    public Text scoreRecord;

    [SerializeField]
    private Text actualPoints;
    public String ActualPoints {
        get { return actualPoints.ToString(); }
        set { actualPoints.text = value; }
    }

    // UI Sound Button
    public GameObject SoundOn;
    public GameObject SoundOff;

    // UI Connection Button
    public GameObject PlayDisconnected;
    public GameObject PlayConnectionPending;
    public GameObject PlayConnectionError;
    public GameObject PlayConnected;

    private void Start() {
        if (null == Instance)
            Instance = this;
    
        if (null != levelSize)
            levelSize.text = ControlManager.Instance.GridSize.ToString() + "x" + ControlManager.Instance.GridSize.ToString();

        if (null != version)
            version.text = ControlManager.Instance.CurrentVersion;

        if (ControlManager.Instance.GPlayConnection) {
            Instance.PlayConnected.SetActive(true);
            Instance.PlayDisconnected.SetActive(false);
        }

        // Auto connect
        //if (false == ControlManager.Instance.GPlayConnection)
        //    BtnGPlayConnection();

        if (!ControlManager.Instance.SoundFx) {
            Instance.SoundOn.SetActive(false);
            Instance.SoundOff.SetActive(true);
        }
    }
    
    // Open settings
    public static void BtnOpenSettings()
    {
        SoundFX.Instance.ClickFX();
        Instance.SettingView.SetActive(true);
        AdManager.ShowInterstitial();
    }

    // Close settings
    public static void BtnCloseSettings()
    {
        SoundFX.Instance.ClickFX();
        Instance.SettingView.SetActive(false);
    }

    // Close settings
    public static void BtnCloseContinue()
    {
        SoundFX.Instance.ClickFX();
        Instance.ContinueOption.SetActive(false);
    }

    public static void ContinueLast()
    {
        //AdManager.Instance.ShowInterstitial();
        SoundFX.Instance.ClickFX();
        //AdsManager.Instance.ShowAd(null);
        StateManager.LoadState();
        ControlManager.Instance.NewGame = false;
        SceneManager.LoadScene("Game");
        Instance.ContinueOption.SetActive(false);
    }

    public static void NotContinueLast()
    {
        //AdManager.Instance.ShowInterstitial();
        SoundFX.Instance.ClickFX();
        ControlManager.Instance.ContinueCount = 0;
        //Game.newMove = 0;
        ControlManager.Instance.NewGame = true;
        ControlManager.Instance.Score = 0;
        SceneManager.LoadScene("Game");
        Instance.ContinueOption.SetActive(false);
    }

    // Open menu
    public static void BtnOpenMenu()
    {
        AdManager.ShowInterstitial();
        SoundFX.Instance.ClickFX();
        Instance.Menu.SetActive(true);
    }

    // Close menu
    public static void BtnCloseMenu()
    {
        SoundFX.Instance.ClickFX();
        Instance.Menu.SetActive(false);
    }

    // Close GameOver
    public static void BtnCloseGameOver()
    {
        SoundFX.Instance.ClickFX();
        Instance.GameOver.SetActive(false);
    }

    public static void BtnRestart()
    {
        AdManager.ShowInterstitial();
        //Game.newMove = 0;
        Instance.RestartPopup.SetActive(true);
        Instance.Menu.SetActive(false);
        SoundFX.Instance.AlertFX();
    }

    public static void BtnCloseRestart()
    {
        Instance.RestartPopup.SetActive(false);
        SoundFX.Instance.ClickFX();
    }

    public static void BtnConfirmRestart()
    {
        /*Script to restart game scene*/
        Instance.RestartPopup.SetActive(false);
        SoundFX.Instance.ClickFX();
        ControlManager.Instance.NewGame = true;
        SceneManager.LoadScene("Game");
    }

    public static void SwitchSound()
    {
        // Call function to Enable/Disable sound
        ControlManager.Instance.UpdateSoundStatus();
    }

    public static void BtnGPlayConnection()
    {
        SoundFX.Instance.OnOffFX();
        if (false == ControlManager.Instance.GPlayConnection)
        {
            ControlManager.Instance.GPlayConnection = true;
            PlayGamesScript.SignIn();
        }
        else if (true == ControlManager.Instance.GPlayConnection)
        {
            ControlManager.Instance.GPlayConnection = false;
            PlayGamesScript.SignOut();
        }
    }

    public static void BtnHome()
    {
        SoundFX.Instance.SpecialContinueExtraFX();
        //AdsManager.Instance.ShowAd(null);
        StateManager.SaveState();
        SceneManager.LoadScene("Main");
        //SSTools.ShowMessage("Button Home Pressed", SSTools.Position.bottom, SSTools.Time.threeSecond);
    }

    /*public GameObject PopupContinue;

    public Text ScoreRecord;

    */

    /*public static void ResetPoints()
    {
        Record = 0;
        UpdatePoints(Record);
        PlayGamesScript.GuardaClassifica();
    }

    public static void BtnShowLadder()
    {
        SoundFX.Instance.ClickFX();
        PlayGamesScript.AggiungiPunteggioClassifica4x4(scoreRecord);
        ResetPoints();
    }*/

    public static void LookLeaderboard()
    {
        PlayGamesScript.GuardaClassifica();
    }

    public static void LookGridLeaderboard()
    {
        PlayGamesScript.GuardaClassificaGrid(ControlManager.Instance.GridSize);
    }

    [System.Obsolete]
    public static void SendEmail()
    {
        string email = "brickpointgames@gmail.com";
        string subject = MyEscapeURL("Feedback New 2048 for Android");
        string body = MyEscapeURL("Hi Brick Point Games,\r\n I've something to tell about New 2048 game for Android.\r");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    [System.Obsolete]
    static string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    public static void OpenPrivacyURL()
    {
        SoundFX.Instance.ClickFX();
        Application.OpenURL("https://www.privacypolicygenerator.info/live.php?token=zTRool3KNsYlwMZQPJTOsvVbBv6FAoH8");
    }

    public static void BtnRateMe()
    {
        SoundFX.Instance.ClickFX();
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.brickpointgames.newdmqo");
    }
}