using System;
using UnityEngine;
using UnityEngine.Advertisements;
public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdsManager instance;
    private string _gameId = "4944535";
    private string _interstitialAds = "Interstitial_Android";
    private string _rewardAds = "Rewarded_Android";
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        
        Advertisement.Initialize(_gameId, false, this);
        Advertisement.Load(_interstitialAds, this);
        Advertisement.Load(_rewardAds, this);

        DontDestroyOnLoad(this);
    }

    #region 初始化Event

    public void OnInitializationComplete()
    {
        Debug.Log("Ads Initialized");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("ads 初始化失败");
    }

    #endregion

    #region 加载Event

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log(placementId + "加载成功");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        throw new System.NotImplementedException();
    }

    #endregion

    #region 展示Event

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        AudioManager.instance.ToggleBgm();
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        AudioManager.instance.ToggleBgm();
    }

    #endregion
    
    /// <summary>
    /// 展示广告
    /// </summary>
    /// <param name="type">0为可跳过广告；1为获利广告</param>
    public void ShowAds(int type)
    {
        switch (type)
        {
            case 0: Advertisement.Show(_interstitialAds, this);
                break;
            case 1: Advertisement.Show(_rewardAds, this);
                break;
        }
    }
    
}
