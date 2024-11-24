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

    private const int DailyBonusCooldownInSeconds = 86400; // 24 часа
    private const int WeeklyBonusCooldownInSeconds = 604800; // 7 дней

    public int countDaily = 5;
    public int countWeekly = 50;

    private void Start()
    {
        // Добавляем слушателей событий на кнопки и запускаем рутину обновления текстов каждые 0.5 секунды
        dailyBonusButton.onClick.AddListener(ClaimDailyBonus);
        weeklyBonusButton.onClick.AddListener(ClaimWeeklyBonus);
        StartCoroutine(UpdateBonusTextsRoutine());
    }

    // Рутина для периодического обновления текстовых элементов
    private IEnumerator UpdateBonusTextsRoutine()
    {
        while (true)
        {
            UpdateBonusTexts();
            yield return new WaitForSeconds(0.5f); // Обновляем каждые 0.5 секунды
        }
    }

    // Обновление текстовых элементов
    private void UpdateBonusTexts()
    {
        // Получаем сохранённые времена последнего получения бонусов
        string dailyBonusTimeStr = PlayerPrefs.GetString(DailyBonusTimeKey, "0");
        string weeklyBonusTimeStr = PlayerPrefs.GetString(WeeklyBonusTimeKey, "0");

        // Парсим время из строкового формата в long
        long dailyBonusTime = long.Parse(dailyBonusTimeStr);
        long weeklyBonusTime = long.Parse(weeklyBonusTimeStr);

        // Получаем текущее время в формате Unix Timestamp
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

        // Вычисляем оставшееся время до следующего ежедневного и еженедельного бонусов
        long dailyCooldown = dailyBonusTime + DailyBonusCooldownInSeconds - currentTimestamp;
        long weeklyCooldown = weeklyBonusTime + WeeklyBonusCooldownInSeconds - currentTimestamp;

        // Обновляем тексты и активность кнопок в зависимости от оставшегося времени
        dailyBonusText.text = FormatTimeDaily(dailyCooldown);
        weeklyBonusText.text = FormatTimeWeekly(weeklyCooldown);

        dailyBonusButton.interactable = dailyCooldown <= 0;
        weeklyBonusButton.interactable = weeklyCooldown <= 0;
    }

    // Форматирование времени для ежедневного бонуса
    private string FormatTimeDaily(long seconds)
    {
        if (seconds <= 0)
        {
            dailyFG.SetActive(false); // Деактивируем элемент, показывающий активность бонуса
            return "Ready"; // Возвращаем сообщение о готовности к получению бонуса
        }
        dailyFG.SetActive(true); // Активируем элемент, показывающий активность бонуса
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    // Форматирование времени для еженедельного бонуса
    private string FormatTimeWeekly(long seconds)
    {
        if (seconds <= 0)
        {
            weeklyFG.SetActive(false); // Деактивируем элемент, показывающий активность бонуса
            return "Ready"; // Возвращаем сообщение о готовности к получению бонуса
        }
        weeklyFG.SetActive(true); // Активируем элемент, показывающий активность бонуса
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D} days {1:D2}:{2:D2}:{3:D2}", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    // Получение ежедневного бонуса
    private void ClaimDailyBonus()
    {
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        GameController.Instance.countStar += countDaily; // Добавляем звёзды игроку
        Data.dataInstance.SaveStarCount(); // Сохраняем количество звёзд
        PlayerPrefs.SetString(DailyBonusTimeKey, currentTimestamp.ToString()); // Сохраняем новое время получения бонуса
        PlayerPrefs.Save(); // Сохраняем изменения

        // Выводим сообщения о получении ежедневного бонуса в консоль для отладки
        Debug.Log("Daily Bonus Claimed!");
        Debug.Log($"New Daily Bonus Time: {currentTimestamp}");
    }

    // Получение еженедельного бонуса
    private void ClaimWeeklyBonus()
    {
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        GameController.Instance.countStar += countWeekly; // Добавляем звёзды игроку
        Data.dataInstance.SaveStarCount(); // Сохраняем количество звёзд
        PlayerPrefs.SetString(WeeklyBonusTimeKey, currentTimestamp.ToString()); // Сохраняем новое время получения бонуса
        PlayerPrefs.Save(); // Сохраняем изменения

        // Выводим сообщения о получении еженедельного бонуса в консоль для отладки
        Debug.Log("Weekly Bonus Claimed!");
        Debug.Log($"New Weekly Bonus Time: {currentTimestamp}");
    }
}
