using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToStart : MonoBehaviour
{
    CanvasElementVisibility visibility;
    void Start()
    {
        visibility = GetComponent<CanvasElementVisibility>();
        GameController.Instance.OnGameStartedChanged += OnGameStarted;
    }

    private void OnGameStarted(bool value)
    {
        if (value)
        {
            visibility.Visible = false;
        }
    }

    private void OnDestroy()
    {
        GameController.Instance.OnGameStartedChanged -= OnGameStarted;
    }
}
