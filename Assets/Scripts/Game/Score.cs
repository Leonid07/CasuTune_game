using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
        //GameController.Instance.OnScoreChanged += UpdateScoreText;
    }

    void UpdateScoreText(int newScore)
    {
        text.text = newScore.ToString();
    }

    private void OnDestroy()
    {
        //GameController.Instance.OnScoreChanged -= UpdateScoreText;
    }
}
