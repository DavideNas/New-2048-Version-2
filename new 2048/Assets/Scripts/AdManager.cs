using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.Events;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set;}
    private const string BANNER_ID = "ca-app-pub-3940256099942544/6300978111";
    private const string INTERSTITIAL_ID = "ca-app-pub-3940256099942544/1033173712";
    private const string REWARDED_VIDEO_ID = "ca-app-pub-3940256099942544/5224354917";

    private BannerView bannerAd;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;

    private AdRequest request;

    private int timesTriedToShowInterstitial;

/*    private void Awake()
    {
        if (null == Instance) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }*/

    private void Start()
    {  
        if (null == Instance)
            Instance = this;
  
        MobileAds.Initialize(InitializationStatusClient => { });
        //request = new AdRequest.Builder().Build();
        this.bannerAd = new BannerView(BANNER_ID, AdSize.SmartBanner, AdPosition.Bottom);
        this.interstitial = new InterstitialAd(INTERSTITIAL_ID);
        this.rewardedAd = new RewardedAd(REWARDED_VIDEO_ID);

        ShowBanner();
    }

    // BANNER MANAGEMENT
    public void ShowBanner()
    {
        GC.Collect();
        request = new AdRequest.Builder().Build();

        this.bannerAd.LoadAd(request);
    }

    public void HideBanner()
    {
        this.bannerAd.Hide();
    }

    // INTERSTITIAL MANAGEMENT
    public void ShowInterstitial()
    {
        GC.Collect();
        request = new AdRequest.Builder().Build();

        this.interstitial.LoadAd(request);

        timesTriedToShowInterstitial++;
        if(this.interstitial.IsLoaded() && timesTriedToShowInterstitial >= 5)
        {
            this.interstitial.Show();
            timesTriedToShowInterstitial = 0;
        }
    }

    // REWARDED VIDEO MANAGEMENT
    public void ShowRewardedVideo()
    {
        GC.Collect();
        request = new AdRequest.Builder().Build();

        this.rewardedAd.LoadAd(request);
        
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        if (this.rewardedAd.IsLoaded()) 
            this.rewardedAd.Show();
    }

    public void HandleUserEarnedReward(object sender, Reward args) 
    {
        UIManager.Instance.GameOver.SetActive(false);
        Brush.Instance.BrushTiles();
        //ControlManager.Instance.ContinueCount--;
        //ManagerScript.Instance.Coins++; 
    }
}