using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using System.Collections;

public class PlayGamesScript : MonoBehaviour
{
    public static PlayGamesScript Instance { get; private set; }

    public static PlayGamesPlatform platform;
    
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
        if (null == platform)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;

            platform = PlayGamesPlatform.Activate();
        }
    }

    public static void SignIn()
    {
        UIManager.Instance.PlayConnectionPending.SetActive(true);
        Social.Active.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                ControlManager.Instance.GPlayConnection = true;
                ControlManager.Instance.SwitchConnection(ControlManager.Instance.GPlayConnection);
                UIManager.Instance.PlayConnectionPending.SetActive(false);
                //SSTools.ShowMessage("Autentication success", SSTools.Position.bottom, SSTools.Time.threeSecond);
            }
            else
            {
                ControlManager.Instance.GPlayConnection = false;
                UIManager.Instance.PlayConnectionPending.SetActive(false);

                UIManager.Instance.PlayConnectionError.SetActive(true);

                //SSTools.ShowMessage("Connection Error", SSTools.Position.bottom, SSTools.Time.threeSecond);

                Instance.Invoke("ConnectionError", 3f);
            }
        });
    }

    void ConnectionError()
    {
        //SSTools.ShowMessage("End Connection Error", SSTools.Position.bottom, SSTools.Time.threeSecond);

        UIManager.Instance.PlayConnectionError.SetActive(false);
        ControlManager.Instance.SwitchConnection(ControlManager.Instance.GPlayConnection);
    }

    public static void SignOut()
    {
        ControlManager.Instance.GPlayConnection = false;
        ControlManager.Instance.SwitchConnection(ControlManager.Instance.GPlayConnection);
        UIManager.Instance.PlayConnectionPending.SetActive(false);
        PlayGamesPlatform.Instance.SignOut();
    }

    #region Leaderboard
    public static void AggiungiPunteggioClassifica3x3(long score)
    {
        Social.ReportScore(score, GPGSIds.leaderboard_3x3, (bool success) =>
        {
            if (success) { }
        });
    }

    public static void AggiungiPunteggioClassifica4x4(long score)
    {
        Social.ReportScore(score, GPGSIds.leaderboard_4x4, (bool success) =>
        {
            if (success) { }
        });
    }

    public static void AggiungiPunteggioClassifica5x5(long score)
    {
        Social.ReportScore(score, GPGSIds.leaderboard_5x5, (bool success) =>
        {
            if (success) { }
        });
    }

    public static void AggiungiPunteggioClassifica6x6(long score)
    {
        Social.ReportScore(score, GPGSIds.leaderboard_6x6, (bool success) => 
        { 
            if (success) { }
        });
    }

    public static void AggiungiPunteggioClassifica8x8(long score)
    {
        Social.ReportScore(score, GPGSIds.leaderboard_8x8, (bool success) =>
        {
            if (success) { }
        });
    }

    public static void CaricaClassifica(int scoreGridRefer)
    {
        switch (scoreGridRefer)
        {
            case 3:
                Social.LoadScores(GPGSIds.leaderboard_3x3, scores => {
                    string myTopScore = "";
                    if (scores.Length > 0)
                    {
                        foreach (IScore score in scores)
                        {
                            if (score.userID == Social.localUser.id)
                            {
                                myTopScore = score.formattedValue.ToString();
                            }
                        }
                        string msg = "my best 3x3: " + myTopScore;
                        //UIManager.ScoreRecord.text = myTopScore;
                    }
                });
                break;
            case 4:
                Social.LoadScores(GPGSIds.leaderboard_4x4, scores => {
                    string myTopScore = "";
                    if (scores.Length > 0)
                    {
                        foreach (IScore score in scores)
                        {
                            if (score.userID == Social.localUser.id)
                            {
                                myTopScore = score.formattedValue.ToString();
                            }
                        }
                        string msg = "my best 4x4: " + myTopScore;
                        //UIManager.ScoreRecord.text = myTopScore;
                    }
                });
                break;
            case 5:
                Social.LoadScores(GPGSIds.leaderboard_5x5, scores => {
                    string myTopScore = "";
                    if (scores.Length > 0)
                    {
                        foreach (IScore score in scores)
                        {
                            if (score.userID == Social.localUser.id)
                            {
                                myTopScore = score.formattedValue.ToString();
                            }
                        }
                        string msg = "my best 5x5: " + myTopScore;
                        //UIManager.ScoreRecord.text = myTopScore;
                    }
                });
                break;
            case 6:
                Social.LoadScores(GPGSIds.leaderboard_6x6, scores => {
                    string myTopScore = "";
                    if (scores.Length > 0)
                    {
                        foreach (IScore score in scores)
                        {
                            if (score.userID == Social.localUser.id)
                            {
                                myTopScore = score.formattedValue.ToString();
                            }
                        }
                        string msg = "my best 6x6: " + myTopScore;
                        //UIManager.ScoreRecord.text = myTopScore;
                    }
                });
                break;
            case 8:
                Social.LoadScores(GPGSIds.leaderboard_8x8, scores => {
                    string myTopScore = "";
                    if (scores.Length > 0)
                    {
                        foreach (IScore score in scores)
                        {
                            if (score.userID == Social.localUser.id)
                            {
                                myTopScore = score.formattedValue.ToString();
                            }
                        }
                        string msg = "my best 8x8: " + myTopScore;
                        //UIManager.ScoreRecord.text = myTopScore;
                    }
                });
                break;
        }
    }

    public static void GuardaClassifica()
    {
        SoundFX.Instance.ClickFX();
        Social.ShowLeaderboardUI();
    }

    public static void GuardaClassificaGrid(int leaderboard_grid)
    {
        if (3 == leaderboard_grid)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_3x3);
        }
        else if (4 == leaderboard_grid)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_4x4);
        }
        else if (5 == leaderboard_grid)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_5x5);
        }
        else if (6 == leaderboard_grid)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_6x6);
        }
        else if (8 == leaderboard_grid)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_8x8);
        }
    }
    #endregion /Leaderboard
}