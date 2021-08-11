using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject PopupContinue;

    public GameObject SettingView;
    
    // UI Sound Button
    public GameObject SoundOn;
    public GameObject SoundOff;

    // UI Connection Button
    public GameObject PlayConnected;
    public GameObject PlayConnectionPending;
    public GameObject PlayConnectionError;
    public GameObject PlayDisconnected;

    public Text ScoreRecord;

    [SerializeField]
    private Text versionTxt;

    public Text Version {
        get { return versionTxt; }
        set { versionTxt = value; }
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

    public static void SwitchSound(bool soundState)
    {
        if (soundState)
        {
            // script to disable sound
            ControlManager.Instance.UpdateSoundStatus(!soundState);
        }
        else
        {
            // script to enable sound
            ControlManager.Instance.UpdateSoundStatus(!soundState);
        }
    }

    // open settings
    public static void BtnOpenSettings()
    {
        SoundFX.Instance.ClickFX();
        Instance.SettingView.SetActive(true);
        Instance.VersionUpdate();
    }

    // close settings
    public static void BtnCloseSettings()
    {
        Instance.SettingView.SetActive(false);
        SoundFX.Instance.ClickFX();
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

    public static void LookGridLeaderboard()
    {
        PlayGamesScript.GuardaClassificaGrid(StageSetting.Instance.GridSize);
    }

    [System.Obsolete]
    public static void SendEmail()
    {
        string email = "brickpointgames@gmail.com";
        string subject = MyEscapeURL("Feedback New 2048 for Android");
        string body = MyEscapeURL("Hi Brick Point Games,\r\n I've something to tell about New 2048 game for Android.\r\n");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    [System.Obsolete]
    static string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    public void VersionUpdate()
    {
        Instance.Version.text = ControlManager.Instance.CurrentVersion;
    }
}