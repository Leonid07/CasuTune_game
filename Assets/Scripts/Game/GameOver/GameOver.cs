using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Image imageToFill; // Ссылка на Image компонент
    public Text countdownText; // Ссылка на Text компонент
    public GameObject gameCamera;
    public Canvas thisCanvas;

    [Header("Параметры для первого окна поражения")]
    public GameObject firstGameOverPanel;
    public GameObject secodGameOverPanel;
    public Image imageTimer;
    public Text textTimer;

    Coroutine coroutine;

    public void Over()
    {
        // Запуск корутины изменения FillAmount
        thisCanvas.sortingOrder = 51;
        gameCamera.SetActive(false);
    }

    public void Con()
    {
        thisCanvas.sortingOrder = 0;
        gameCamera.SetActive(true);
    }

    public void StartCouroutine()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(FillImageOverTime(imageTimer, textTimer, 5f));
    }

    public void StopCouroutine()
    {
        StopCoroutine(FillImageOverTime(imageTimer, textTimer, 5f));
    }

    public IEnumerator FillImageOverTime(Image image, Text countdownText, float duration)
    {
        float elapsedTime = 0f;
        image.fillAmount = 0f; // Начальная заполненность

        while (elapsedTime < duration)
        {
            image.fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / duration);

            // Обратный отсчёт
            int remainingTime = Mathf.CeilToInt(duration - elapsedTime);
            countdownText.text = remainingTime.ToString(); // Выводить только целые числа

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Убедиться, что fillAmount установлен точно в 1
        image.fillAmount = 1f;

        // Убедиться, что обратный отсчёт установлен точно в 0
        countdownText.text = "0";
        firstGameOverPanel.SetActive(false);
        secodGameOverPanel.SetActive(true);
    }
}
