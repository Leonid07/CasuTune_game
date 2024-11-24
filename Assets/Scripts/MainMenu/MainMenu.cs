using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anim;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public RectTransform rectHome;
    public RectTransform rectShop;
    public RectTransform rectBonus;
    public RectTransform rectSetting;

    public Button buttonHome;
    public Button buttonShop;
    public Button buttonBonus;
    public Button buttonSetting;

    public Record[] records;
    public SwipePanel swipePanel;

    public int[] bpm;

    public void SetAudioFromRecord()
    {
        int index = swipePanel.currentPage;
        index--;
        GameController.Instance.bpm = bpm[index];
        GameController.Instance.audioSource = records[index].audioSource;
    }
}
