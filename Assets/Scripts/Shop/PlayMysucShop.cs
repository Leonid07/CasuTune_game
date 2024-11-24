using Anim;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayMysucShop : MonoBehaviour
{
    // animation
    public RectTransform[] record;// Массив RectTransform для анимации пластинок
    public Button[] buttonRecord;// Массив кнопок для запуска анимации и музыки
    public float speedRotate = 35;// Скорость вращения пластинки

    // music
    public Sounds sounds;// Класс звуков
    public AudioSource[] audioSource;// Массив аудио источников для воспроизведения музыки


    private Coroutine currentCoroutine = null;// Текущая запущенная корутина
    private int currentPlayingIndex = -1;// Индекс текущей воспроизводимой музыки

    private void Start()
    {
        // Присваиваем действия кнопкам
        for (int i = 0; i < record.Length; i++)
        {
            int index = i;
            buttonRecord[i].onClick.AddListener(() => PlayMusic(index)); // Добавляем слушатель клика для каждой кнопки
        }
    }

    void PlayMusic(int number)
    {
        // Остановить текущее аудио, если оно воспроизводится
        if (currentPlayingIndex != -1 && audioSource[currentPlayingIndex].isPlaying)
        {
            audioSource[currentPlayingIndex].Stop(); // Остановить текущий аудиотрек
        }

        // Остановить текущую корутину, если она запущена
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine); // Остановить текущую корутину
        }

        // Запуск новой анимации и аудио
        currentCoroutine = StartCoroutine(UtillsAnim.RotateImageAnim(record[number], speedRotate)); // Запустить корутину для анимации вращения пластинки
        audioSource[number].Play(); // Воспроизвести аудиотрек
        currentPlayingIndex = number; // Установить текущий индекс воспроизводимого трека
    }

    public void StopAllMusic()
    {
        // Остановить все аудиотреки
        for (int i = 0; i < audioSource.Length; i++)
        {
            audioSource[i].Stop(); // Остановить каждый аудиотрек
        }
    }
}
