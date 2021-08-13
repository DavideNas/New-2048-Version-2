using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class SoundFX : MonoBehaviour
{
    public static SoundFX Instance { get; private set; }

    // Menu FX
    private AudioSource m_SwipeMenuFX;
    private AudioSource m_ClickFX;
    private AudioSource m_Alert;
    private AudioSource m_OnOff;

    // Game FX
    private AudioSource m_MatchTiles;
    private AudioSource m_UndoSpecial;
    private AudioSource m_ContinueSpecial;
    private AudioSource m_GameOver;
    
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
        // preload SwipeMenuFX
        m_SwipeMenuFX = gameObject.AddComponent<AudioSource>();
        m_SwipeMenuFX.clip = Resources.Load("LevelSelect") as AudioClip;

        // preload ClickButton FX
        m_ClickFX = gameObject.AddComponent<AudioSource>();
        m_ClickFX.clip = Resources.Load("PushBtn") as AudioClip;

        // preload Alert FX
        m_Alert = gameObject.AddComponent<AudioSource>();
        m_Alert.clip = Resources.Load("Alert") as AudioClip;

        // preload On Off FX
        m_OnOff = gameObject.AddComponent<AudioSource>();
        m_OnOff.clip = Resources.Load("OnOff") as AudioClip;

        // preload Match Tiles Pop FX
        m_MatchTiles = gameObject.AddComponent<AudioSource>();
        m_MatchTiles.clip = Resources.Load("MatchTilesPop") as AudioClip;

        // preload Special Undo FX
        m_UndoSpecial = gameObject.AddComponent<AudioSource>();
        m_UndoSpecial.clip = Resources.Load("SpecialMoveUndo") as AudioClip;

        // preload Special Continue FX
        m_ContinueSpecial = gameObject.AddComponent<AudioSource>();
        m_ContinueSpecial.clip = Resources.Load("SpecialMoveContinue") as AudioClip;

        // preload Game Over FX
        m_GameOver = gameObject.AddComponent<AudioSource>();
        m_GameOver.clip = Resources.Load("Alert") as AudioClip;
    }

    public void SelectLevelFX()
    {
        if (ControlManager.Instance.SoundFx)
            m_SwipeMenuFX.Play();
    }

    public void ClickFX()
    {
        Debug.Log(ControlManager.Instance.SoundFx);
        if (ControlManager.Instance.SoundFx)
            m_ClickFX.Play();
    }

    public void AlertFX()
    {
        if (ControlManager.Instance.SoundFx)
            m_Alert.Play();
    }
    
    public void OnOffFX()
    {
        if (ControlManager.Instance.SoundFx)
            m_OnOff.Play();
    }

    public void MatchTilesFX()
    {
        if (ControlManager.Instance.SoundFx)
            m_MatchTiles.Play();
    }

    public void SpecialUndoFX()
    {
        if (ControlManager.Instance.SoundFx)
            m_UndoSpecial.Play();
    }

    public void SpecialContinueExtraFX()
    {
        if (ControlManager.Instance.SoundFx)
            m_ContinueSpecial.Play();
    }
    
    public void GameOverFX()
    {
        if (ControlManager.Instance.SoundFx)
            m_GameOver.Play();
    }
}
