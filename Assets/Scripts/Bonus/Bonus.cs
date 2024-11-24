using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Bonus : MonoBehaviour
{
    public TMP_Text dailyBonusText;
    public TMP_Text weeklyBonusText;
    public Button dailyBonusButton;
    public Button weeklyBonusButton;

    public GameObject dailyFG;
    public GameObject weeklyFG;

    private const string DailyBonusTimeKey = "daily_bonus_time";
    private const string WeeklyBonusTimeKey = "weekly_bonus_time";

    private const int DailyBonusCooldownInSeconds = 86400; // 24 ����
    private const int WeeklyBonusCooldownInSeconds = 604800; // 7 ����

    public int countDaily = 5;
    public int countWeekly = 50;

    private void Start()
    {
        // ��������� ���������� ������� �� ������ � ��������� ������ ���������� ������� ������ 0.5 �������
        dailyBonusButton.onClick.AddListener(ClaimDailyBonus);
        weeklyBonusButton.onClick.AddListener(ClaimWeeklyBonus);
        StartCoroutine(UpdateBonusTextsRoutine());
    }

    // ������ ��� �������������� ���������� ��������� ���������
    private IEnumerator UpdateBonusTextsRoutine()
    {
        while (true)
        {
            UpdateBonusTexts();
            yield return new WaitForSeconds(0.5f); // ��������� ������ 0.5 �������
        }
    }

    // ���������� ��������� ���������
    private void UpdateBonusTexts()
    {
        // �������� ���������� ������� ���������� ��������� �������
        string dailyBonusTimeStr = PlayerPrefs.GetString(DailyBonusTimeKey, "0");
        string weeklyBonusTimeStr = PlayerPrefs.GetString(WeeklyBonusTimeKey, "0");

        // ������ ����� �� ���������� ������� � long
        long dailyBonusTime = long.Parse(dailyBonusTimeStr);
        long weeklyBonusTime = long.Parse(weeklyBonusTimeStr);

        // �������� ������� ����� � ������� Unix Timestamp
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

        // ��������� ���������� ����� �� ���������� ����������� � ������������� �������
        long dailyCooldown = dailyBonusTime + DailyBonusCooldownInSeconds - currentTimestamp;
        long weeklyCooldown = weeklyBonusTime + WeeklyBonusCooldownInSeconds - currentTimestamp;

        // ��������� ������ � ���������� ������ � ����������� �� ����������� �������
        dailyBonusText.text = FormatTimeDaily(dailyCooldown);
        weeklyBonusText.text = FormatTimeWeekly(weeklyCooldown);

        dailyBonusButton.interactable = dailyCooldown <= 0;
        weeklyBonusButton.interactable = weeklyCooldown <= 0;
    }

    // �������������� ������� ��� ����������� ������
    private string FormatTimeDaily(long seconds)
    {
        if (seconds <= 0)
        {
            dailyFG.SetActive(false); // ������������ �������, ������������ ���������� ������
            return "Ready"; // ���������� ��������� � ���������� � ��������� ������
        }
        dailyFG.SetActive(true); // ���������� �������, ������������ ���������� ������
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    // �������������� ������� ��� ������������� ������
    private string FormatTimeWeekly(long seconds)
    {
        if (seconds <= 0)
        {
            weeklyFG.SetActive(false); // ������������ �������, ������������ ���������� ������
            return "Ready"; // ���������� ��������� � ���������� � ��������� ������
        }
        weeklyFG.SetActive(true); // ���������� �������, ������������ ���������� ������
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D} days {1:D2}:{2:D2}:{3:D2}", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    // ��������� ����������� ������
    private void ClaimDailyBonus()
    {
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        GameController.Instance.countStar += countDaily; // ��������� ����� ������
        Data.dataInstance.SaveStarCount(); // ��������� ���������� ����
        PlayerPrefs.SetString(DailyBonusTimeKey, currentTimestamp.ToString()); // ��������� ����� ����� ��������� ������
        PlayerPrefs.Save(); // ��������� ���������

        // ������� ��������� � ��������� ����������� ������ � ������� ��� �������
        Debug.Log("Daily Bonus Claimed!");
        Debug.Log($"New Daily Bonus Time: {currentTimestamp}");
    }

    // ��������� ������������� ������
    private void ClaimWeeklyBonus()
    {
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        GameController.Instance.countStar += countWeekly; // ��������� ����� ������
        Data.dataInstance.SaveStarCount(); // ��������� ���������� ����
        PlayerPrefs.SetString(WeeklyBonusTimeKey, currentTimestamp.ToString()); // ��������� ����� ����� ��������� ������
        PlayerPrefs.Save(); // ��������� ���������

        // ������� ��������� � ��������� ������������� ������ � ������� ��� �������
        Debug.Log("Weekly Bonus Claimed!");
        Debug.Log($"New Weekly Bonus Time: {currentTimestamp}");
    }
}
