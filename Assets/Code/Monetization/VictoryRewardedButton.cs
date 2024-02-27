using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine;
using UnityEngine.UI;

public class VictoryRewardedButton : MonoBehaviour, IRewardedVideoAdListener
{
    [SerializeField] private Button rewarded;
    void Start()
    {
        rewarded.onClick.AddListener(ShowRewardedAds);
    }

    public void ShowRewardedAds()
    {
        Appodeal.setRewardedVideoCallbacks(this);
        
        if(Appodeal.isLoaded(Appodeal.REWARDED_VIDEO)) {
            Appodeal.show(Appodeal.REWARDED_VIDEO);
        }
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
        //Добавлять монеты
        
        Debug.Log("RewardedVideo finished");
    }

//Called when rewarded video is expired and can not be shown
    public void onRewardedVideoExpired()
    {
        Debug.Log("RewardedVideo expired");
    }

    #endregion
}

