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
    //public static AdManager Instance { get; private set;}

    // FOR GOOGLE PLAY STORE
    private const string BANNER_ID = "ca-app-pub-9352383645515305/2724497699";
    private const string INTERSTITIAL_ID = "ca-app-pub-9352383645515305/5350661032";
    private const string REWARDED_VIDEO_ID = "ca-app-pub-9352383645515305/2477085682";
    

    // FOR INTERNAL TESTING
    /*private const string BANNER_ID = "ca-app-pub-3940256099942544/6300978111";
    private const string INTERSTITIAL_ID = "ca-app-pub-3940256099942544/1033173712";
    private const string REWARDED_VIDEO_ID = "ca-app-pub-3940256099942544/5224354917";*/

    private static BannerView bannerAd;
    private static InterstitialAd interstitial;
    private static RewardedAd rewardedAd;

    private static AdRequest request;

    private static int timesTriedToShowInterstitial;

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
        /*if (null == Instance)
            Instance = this;*/
  
        MobileAds.Initialize(InitializationStatusClient => { });
        //request = new AdRequest.Builder().Build();
        bannerAd = new BannerView(BANNER_ID, AdSize.SmartBanner, AdPosition.Bottom);
        interstitial = new InterstitialAd(INTERSTITIAL_ID);
        rewardedAd = new RewardedAd(REWARDED_VIDEO_ID);

        //ShowBanner();
    }

    // BANNER MANAGEMENT
    public void ShowBanner()
    {
        GC.Collect();
        request = new AdRequest.Builder().Build();

        bannerAd.LoadAd(request);
    }

    public void HideBanner()
    {
        bannerAd.Hide();
    }

    // INTERSTITIAL MANAGEMENT
    public static void ShowInterstitial()
    {
        GC.Collect();
        request = new AdRequest.Builder().Build();

        interstitial.LoadAd(request);

        timesTriedToShowInterstitial++;
        if(interstitial.IsLoaded() && timesTriedToShowInterstitial >= 5)
        {
            interstitial.Show();
            timesTriedToShowInterstitial = 0;
        }
    }

    // REWARDED VIDEO MANAGEMENT
    public static void ShowRewardedVideo()
    {
        GC.Collect();
        request = new AdRequest.Builder().Build();

        rewardedAd.LoadAd(request);
        
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        if (rewardedAd.IsLoaded()) 
            rewardedAd.Show();
    }

    public static void HandleUserEarnedReward(object sender, Reward args) 
    {
        UIManager.Instance.GameOver.SetActive(false);
        Brush.Instance.BrushTiles();
        //ControlManager.Instance.ContinueCount--;
        //ManagerScript.Instance.Coins++; 
    }
}