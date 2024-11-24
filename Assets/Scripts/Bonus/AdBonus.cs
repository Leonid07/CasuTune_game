using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdBonus : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public Text welcomeBonusText;
    public Button welcomeBonusButton;

    public GameObject welcomeFG;

    public int countStar = 10;

    private const string welcomeBonusTimeKey = "RewardForAds_bonus_time";
    private const int welcomeBonusCooldownInSeconds = 480; // 8 minutes

    [SerializeField] string adPlacementIdIOS = "Rewarded_iOS";
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    string _adUnitId = null;

    private void Awake()
    {
#if UNITY_IOS
        _adUnitId = adPlacementIdIOS;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif
    }

    private void Start()
    {
        OnUnityAdsAdLoaded(_adUnitId);

        welcomeBonusButton.onClick.AddListener(ClaimDailyBonus);
        StartCoroutine(UpdateBonusTextsRoutine());
    }
    public void LoadAd()
    {
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }
    public void ShowAd()
    {
        Advertisement.Show(_adUnitId, this);
    }
    private IEnumerator UpdateBonusTextsRoutine()
    {
        while (true)
        {
            UpdateBonusTexts();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void UpdateBonusTexts()
    {
        string dailyBonusTimeStr = PlayerPrefs.GetString(welcomeBonusTimeKey, "0");
        long dailyBonusTime = long.Parse(dailyBonusTimeStr);
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        long dailyCooldown = dailyBonusTime + welcomeBonusCooldownInSeconds - currentTimestamp;
        welcomeBonusText.text = FormatTimeDaily(dailyCooldown);
        welcomeBonusButton.interactable = dailyCooldown <= 0;
    }

    private string FormatTimeDaily(long seconds)
    {
        if (seconds <= 0)
        {
            welcomeFG.SetActive(false);
            return "Ready";
        }
        welcomeFG.SetActive(true);
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    private void ClaimDailyBonus()
    {
        LoadAd();
        OnUnityAdsAdLoaded(_adUnitId);
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        PlayerPrefs.SetString(welcomeBonusTimeKey, currentTimestamp.ToString());
        PlayerPrefs.Save();

        UpdateBonusTexts();
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_adUnitId))
        {
            welcomeBonusButton.interactable = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            GameController.Instance.countStar += countStar;
            Data.dataInstance.SaveStarCount();
            ClaimDailyBonus();
        }
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {
        welcomeBonusButton.onClick.RemoveAllListeners();
    }
}