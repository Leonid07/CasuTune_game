using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.Windows;
using static System.Net.Mime.MediaTypeNames;
public class RecordInShop : MonoBehaviour
{
    public Button buttonBuy;
    public TMP_Text textCount;
    public TMP_Text text;
    public Record record;

    public int isBuyButton = 0; // 0 куплно 1 не куплено

    private void Start()
    {
        buttonBuy.onClick.AddListener(Buy);
        if (record.isBuy == 1)
        {
            buttonBuy.gameObject.SetActive(false);
        }
    }

    private void Buy()
    {
        Match match = Regex.Match(textCount.text, @"\d+");
        int count = int.Parse(match.Value);
        if (GameController.Instance.countStar >= count)
        {
            record.isBuy = 1;
            isBuyButton++;
            GameController.Instance.countStar -= count;
            int value = Convert.ToInt32(text.text) - count;
            text.text = value.ToString();
            Data.dataInstance.SaveRecordInShop();
            buttonBuy.gameObject.SetActive(false);
            Data.dataInstance.SaveStarCount();
            Data.dataInstance.mainMenu.swipePanel.CheckIsBuyRecord();
            Data.dataInstance.SaveButtonInShop();
        }
    }
}
