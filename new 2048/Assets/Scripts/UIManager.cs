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

    public Text levelSize;

    public Text version;

    public Text scoreRecord;
    /*[SerializeField]
    private Text scoreRecord;
    public String ScoreRecord {
        get { return scoreRecord.ToString(); }
        set { scoreRecord.text = value; }
    }*/

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
        if (false == ControlManager.Instance.GPlayConnection)
            BtnGPlayConnection();

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
    }

    // Close settings
    public static void BtnCloseSettings()
    {
        SoundFX.Instance.ClickFX();
        Instance.SettingView.SetActive(false);
    }

    // Open menu
    public static void BtnOpenMenu()
    {
        SoundFX.Instance.ClickFX();
        Instance.Menu.SetActive(true);
    }

    // Close menu
    public static void BtnCloseMenu()
    {
        SoundFX.Instance.ClickFX();
        Instance.Menu.SetActive(false);
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
        SoundFX.Instance.ClickFX();
        //AdsManager.Instance.ShowAd(null);
        //Utils.SaveState();
        SceneManager.LoadScene("Main");
    }

    /*public GameObject PopupContinue;

    public Text ScoreRecord;

    */

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

    public static void LookGridLeaderboard()
    {
        PlayGamesScript.GuardaClassificaGrid(ControlManager.Instance.GridSize);
    }

    public static void BtnRateMe()
    {
        SoundFX.Instance.ClickFX();
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.brickpointgames.newdmqo");
    }
}