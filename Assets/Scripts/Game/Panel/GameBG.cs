using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameBG : MonoBehaviour
{
    public Image[] imageBG; // Массив изображений
    public float changeInterval = 5f; // Интервал изменения изображений в секундах
    public float fadeDuration = 1f; // Продолжительность плавного перехода в секундах
    public Text textScore;
    public Slider sliderStar;

    private Coroutine bgAnimationCoroutine;
    private int currentIndex = 0;

    public void StartAnimGB()
    {
        // Убедиться, что массив изображений не пуст
        if (imageBG == null || imageBG.Length == 0)
        {
            return;
        }

        // Установить начальную прозрачность всех изображений, кроме первого
        for (int i = 1; i < imageBG.Length; i++)
        {
            SetImageAlpha(imageBG[i], 0f);
        }

        // Запуск анимации смены изображений
        bgAnimationCoroutine = StartCoroutine(SwitchImages());
    }

    void OnDestroy()
    {
        // Остановка корутины при уничтожении объекта
        if (bgAnimationCoroutine != null)
            StopCoroutine(bgAnimationCoroutine);
    }

    IEnumerator SwitchImages()
    {
        while (true)
        {
            int nextIndex = (currentIndex + 1) % imageBG.Length;

            // Плавное затухание текущего изображения и появление следующего
            yield return StartCoroutine(FadeOutIn(imageBG[currentIndex], imageBG[nextIndex]));

            // Обновление текущего индекса
            currentIndex = nextIndex;

            // Ожидание перед следующей сменой изображения
            yield return new WaitForSeconds(changeInterval);
        }
    }

    IEnumerator FadeOutIn(Image currentImage, Image nextImage)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float newAlphaOut = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            float newAlphaIn = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);

            SetImageAlpha(currentImage, newAlphaOut);
            SetImageAlpha(nextImage, newAlphaIn);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Установка окончательной прозрачности
        SetImageAlpha(currentImage, 0f);
        SetImageAlpha(nextImage, 1f);
    }

    void SetImageAlpha(Image image, float alpha)
    {
        // Получение текущего цвета изображения и установка новой альфа-компоненты
        Color imageColor = image.color;
        imageColor.a = alpha;
        image.color = imageColor;
    }
}