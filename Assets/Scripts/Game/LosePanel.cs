using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class LosePanel : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameController.Instance.EndGame();
        GameController.Instance.Over.Over();
        GameController.Instance.Over.StartCouroutine();
        gameObject.SetActive(false);
    }
}
