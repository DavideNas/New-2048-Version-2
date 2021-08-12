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

    public Text levelSize;

    public Text version;

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
    }
    
    // Open settings
    public static void BtnOpenSettings()
    {
        SoundFX.Instance.ClickFX();
        Instance.SettingView.SetActive(true);
        //VersionUpdate();
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
        //VersionUpdate();
    }

    // Close menu
    public static void BtnCloseMenu()
    {
        SoundFX.Instance.ClickFX();
        Instance.Menu.SetActive(false);
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

/*  public static void VersionUpdate()
    {
        version.text = ControlManager.Instance.CurrentVersion;
    }*/

    /*public GameObject PopupContinue;

    // UI Sound Button
    public GameObject SoundOn;
    public GameObject SoundOff;

    public Text ScoreRecord;

    [SerializeField]
    private Text versionTxt;

    public Text Version {
        get { return versionTxt; }
        set { versionTxt = value; }
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
*/

/*    public static void LookGridLeaderboard()
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
*/
}