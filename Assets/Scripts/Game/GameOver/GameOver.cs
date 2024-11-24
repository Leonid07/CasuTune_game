using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Image imageToFill; // ������ �� Image ���������
    public Text countdownText; // ������ �� Text ���������
    public GameObject gameCamera;
    public Canvas thisCanvas;

    [Header("��������� ��� ������� ���� ���������")]
    public GameObject firstGameOverPanel;
    public GameObject secodGameOverPanel;
    public Image imageTimer;
    public Text textTimer;

    Coroutine coroutine;

    public void Over()
    {
        // ������ �������� ��������� FillAmount
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
        image.fillAmount = 0f; // ��������� �������������

        while (elapsedTime < duration)
        {
            image.fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / duration);

            // �������� ������
            int remainingTime = Mathf.CeilToInt(duration - elapsedTime);
            countdownText.text = remainingTime.ToString(); // �������� ������ ����� �����

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���������, ��� fillAmount ���������� ����� � 1
        image.fillAmount = 1f;

        // ���������, ��� �������� ������ ���������� ����� � 0
        countdownText.text = "0";
        firstGameOverPanel.SetActive(false);
        secodGameOverPanel.SetActive(true);
    }
}
