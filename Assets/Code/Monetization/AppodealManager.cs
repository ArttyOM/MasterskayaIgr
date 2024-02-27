using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using TMPro;
public class AppodealManager : MonoBehaviour, IRewardedVideoAdListener
{
    // Ключ приложения 
    private const string APP_KEY = "";
    
    //Режим тестовой рекламы
    [SerializeField] private bool testingMode;
   // private IRewardedVideoAdListener _rewardedVideoAdListenerImplementation;

    void Start()
    {
        Initialized();
    }

    

    public void ShowRewardedAds()
    {
        if(Appodeal.isLoaded(Appodeal.REWARDED_VIDEO)) {
            Appodeal.show(Appodeal.REWARDED_VIDEO);
        }
    }
    private void Initialized()
    {
        Appodeal.setTesting(testingMode);
        
        Appodeal.disableLocationPermissionCheck();
        
        Appodeal.muteVideosIfCallsMuted(true);
        
        Appodeal.initialize(APP_KEY, Appodeal.INTERSTITIAL| Appodeal.REWARDED_VIDEO);
        
        Appodeal.setRewardedVideoCallbacks(this);
    }

    #region Rewarded Video callback handlers

//Called when rewarded video was loaded (precache flag shows if the loaded ad is precache).
    public void onRewardedVideoLoaded(bool isPrecache)
    {
        Debug.Log("RewardedVideo loaded");
    }

// Called when rewarded video failed to load
    public void onRewardedVideoFailedToLoad()
    {
        Debug.Log("RewardedVideo failed to load");
    }

// Called when rewarded video was loaded, but cannot be shown (internal network errors, placement settings, etc.)
    public void onRewardedVideoShowFailed()
    {
        Debug.Log("RewardedVideo show failed");
    }

// Called when rewarded video is shown
    public void onRewardedVideoShown()
    {
        Debug.Log("RewardedVideo shown");
    }

// Called when reward video is clicked
    public void onRewardedVideoClicked()
    {
        Debug.Log("RewardedVideo clicked");
    }

// Called when rewarded video is closed
    public void onRewardedVideoClosed(bool finished)
    {
        Debug.Log("RewardedVideo closed");
    }

// Called when rewarded video is viewed until the end
    public void onRewardedVideoFinished(double amount, string name)
    {
        Debug.Log("RewardedVideo finished");
    }

//Called when rewarded video is expired and can not be shown
    public void onRewardedVideoExpired()
    {
        Debug.Log("RewardedVideo expired");
    }

    #endregion
}
