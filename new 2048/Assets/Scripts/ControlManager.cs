using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public static ControlManager Instance { get; private set; }

    // Version control
    private string currentVersion;
    public string CurrentVersion {
        get { return currentVersion; }
        set { currentVersion = value; }
    }

    // Google connection status
    private bool googlePlayConnection;
    public bool GPlayConnection{ 
        get { return googlePlayConnection; }
        set { googlePlayConnection = value; }
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

    private void Start()
    {
        CurrentVersion = Application.version + " - 23 (Main Release)";
    }

    public void UpdateSoundStatus(bool soundStatus)
    {
        if (soundStatus)
        {
            UIManager.Instance.SoundOn.SetActive(false);
            UIManager.Instance.SoundOff.SetActive(true);
        }
        else
        {
            UIManager.Instance.SoundOn.SetActive(true);
            UIManager.Instance.SoundOff.SetActive(false);
        }
    }

    public void SwitchConnection(bool connectionState)
    {
        if (connectionState)
        {
            // script to disable connection
            UIManager.Instance.PlayConnected.SetActive(true);
            UIManager.Instance.PlayDisconnected.SetActive(false);
        }
        else
        {
            // script to enable connection
            UIManager.Instance.PlayConnected.SetActive(false);
            UIManager.Instance.PlayDisconnected.SetActive(true);
        }
    }
}
