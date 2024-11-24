using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Text score;
    private CanvasElementVisibility visibility;
    public CanvasElementVisibility winnerPraise;

    void Start()
    {
        visibility = GetComponent<CanvasElementVisibility>();
        GameController.Instance.OnShowGameOverScreenChanged += OnShowGameOverScreenChanged;
    }

    private void OnShowGameOverScreenChanged(bool value)
    {
        if (value)
        {
            visibility.Visible = true;
            //score.text = GameController.Instance.Score.ToString(); /////  нужно сделать счёт
            winnerPraise.Visible = GameController.Instance.PlayerWon;
        }
    }

    private void OnDestroy()
    {
        GameController.Instance.OnShowGameOverScreenChanged -= OnShowGameOverScreenChanged;
    }
}
