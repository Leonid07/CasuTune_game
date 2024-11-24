using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Data : MonoBehaviour
{
    public RecordsData[] recordsData;
    public static Data dataInstance { get; private set; }

    public MainMenu mainMenu;

    public string IdCountStar = "IdCountStar";

    public Sprite starYellow;

    private void Awake()
    {
        if (dataInstance == null)
        {
            dataInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        idIsBuyRecord = new int[recordsData.Length];
        idButtonRecord = new int[GameController.Instance.recordInShops.Length];
        InitializeRecordsData();
        LoadStarCount();
        LoadRecoredStar();
        LoadRecordInShop();
        LoadButtonInShop();
    }

    private void InitializeRecordsData()
    {
        int idCount = 0;
        for (int i = 0; i < recordsData.Length; i++)
        {
            if (mainMenu.records != null && mainMenu.records.Length > i && mainMenu.records[i].star != null)
            {
                int starCountLength = mainMenu.records[i].star.Count;

                recordsData[i].starCount = new int[starCountLength]; // Задайте необходимую длину массива starCount
                recordsData[i].IdStar = new string[starCountLength]; // Задайте необходимую длину массива IdStar

                for (int j = 0; j < starCountLength; j++)
                {
                    recordsData[i].starCount[j] = 0; // Инициализация starCount значениями по умолчанию

                    recordsData[i].IdStar[j] = idCount.ToString(); // Использование Guid для уникальных идентификаторов
                    idCount++;
                }
            }
        }
    }

    public void SaveRecordStar()
    {
        int idCount = 0;
        for (int q = 0; q < mainMenu.records.Length; q++)
        {
            for (int j = 0; j < mainMenu.records[q].star.Count; j++)
            {
                if (mainMenu.records[q].star[j].sprite == starYellow)
                {
                    recordsData[q].starCount[j]++;
                    // Проверяем, был ли уже сохранен IDStar
                    if (string.IsNullOrEmpty(recordsData[q].IdStar[j]))
                    {
                        recordsData[q].IdStar[j] = idCount.ToString();
                        idCount++;
                    }
                    PlayerPrefs.SetInt(recordsData[q].IdStar[j], recordsData[q].starCount[j]);
                }
            }
        }
        PlayerPrefs.Save();
    }

    public void LoadRecoredStar()
    {
        for (int q = 0; q < mainMenu.records.Length; q++)
        {
            for (int j = 0; j < mainMenu.records[q].star.Count; j++)
            {
                if (!string.IsNullOrEmpty(recordsData[q].IdStar[j]) && PlayerPrefs.HasKey(recordsData[q].IdStar[j]))
                {
                    recordsData[q].starCount[j] = PlayerPrefs.GetInt(recordsData[q].IdStar[j]);
                    if (recordsData[q].starCount[j] > 0)
                    {
                        mainMenu.records[q].star[j].sprite = starYellow;
                    }
                }
            }
        }
    }


    public void SaveStarCount()
    {
        PlayerPrefs.SetInt(IdCountStar, GameController.Instance.countStar);
        PlayerPrefs.Save();
    }

    public void LoadStarCount()
    {
        if (PlayerPrefs.HasKey(IdCountStar))
        {
            GameController.Instance.countStar = PlayerPrefs.GetInt(IdCountStar);
        }
    }


    public int[] idIsBuyRecord;
    public void LoadRecordInShop()
    {
        int idCount = 1000;
        for (int i = 0; i < mainMenu.records.Length; i++)
        {
            if (PlayerPrefs.HasKey(idCount.ToString()))
            {
                mainMenu.records[i].isBuy = PlayerPrefs.GetInt(idCount.ToString());
                idIsBuyRecord[i] = idCount;
            }
            idCount++;
        }
        PlayerPrefs.Save();
    }
    public void SaveRecordInShop()
    {
        int idCount = 1000;
        for (int i = 0; i < mainMenu.records.Length; i++)
        {
            idIsBuyRecord[i] = idCount;
            PlayerPrefs.SetInt(idCount.ToString(), mainMenu.records[i].isBuy);
            idCount++;
        }
        PlayerPrefs.Save();
    }

    public int[] idButtonRecord;
    public void SaveButtonInShop()
    {
        int idCount = 10000;
        for (int i = 0; i < GameController.Instance.recordInShops.Length; i++)
        {
            idIsBuyRecord[i] = idCount;
            PlayerPrefs.SetInt(idCount.ToString(), GameController.Instance.recordInShops[i].isBuyButton);
            idCount++;
        }
        PlayerPrefs.Save();
    }
    public void LoadButtonInShop()
    {
        int idCount = 10000;
        for (int i = 0; i < GameController.Instance.recordInShops.Length; i++)
        {
            idIsBuyRecord[i] = idCount;
            if (PlayerPrefs.HasKey(idCount.ToString()))
            {
                GameController.Instance.recordInShops[i].isBuyButton = PlayerPrefs.GetInt(idCount.ToString());
            }
            idCount++;
        }
        PlayerPrefs.Save();
    }

    [Serializable] public struct RecordsData
    {
        public Image[] star;
        public int[] starCount;// 0 нет 1 есть
        public string[] IdStar;
    }
}
