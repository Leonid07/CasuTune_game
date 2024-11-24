using Anim;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayMysucShop : MonoBehaviour
{
    // animation
    public RectTransform[] record;// ������ RectTransform ��� �������� ���������
    public Button[] buttonRecord;// ������ ������ ��� ������� �������� � ������
    public float speedRotate = 35;// �������� �������� ���������

    // music
    public Sounds sounds;// ����� ������
    public AudioSource[] audioSource;// ������ ����� ���������� ��� ��������������� ������


    private Coroutine currentCoroutine = null;// ������� ���������� ��������
    private int currentPlayingIndex = -1;// ������ ������� ��������������� ������

    private void Start()
    {
        // ����������� �������� �������
        for (int i = 0; i < record.Length; i++)
        {
            int index = i;
            buttonRecord[i].onClick.AddListener(() => PlayMusic(index)); // ��������� ��������� ����� ��� ������ ������
        }
    }

    void PlayMusic(int number)
    {
        // ���������� ������� �����, ���� ��� ���������������
        if (currentPlayingIndex != -1 && audioSource[currentPlayingIndex].isPlaying)
        {
            audioSource[currentPlayingIndex].Stop(); // ���������� ������� ���������
        }

        // ���������� ������� ��������, ���� ��� ��������
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine); // ���������� ������� ��������
        }

        // ������ ����� �������� � �����
        currentCoroutine = StartCoroutine(UtillsAnim.RotateImageAnim(record[number], speedRotate)); // ��������� �������� ��� �������� �������� ���������
        audioSource[number].Play(); // ������������� ���������
        currentPlayingIndex = number; // ���������� ������� ������ ���������������� �����
    }

    public void StopAllMusic()
    {
        // ���������� ��� ����������
        for (int i = 0; i < audioSource.Length; i++)
        {
            audioSource[i].Stop(); // ���������� ������ ���������
        }
    }
}
