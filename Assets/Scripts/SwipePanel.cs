using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipePanel : MonoBehaviour, IEndDragHandler
{
    [SerializeField] int maxPage;
    public int currentPage;
    Vector3 targetPos;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform levelPagesRect;

    [SerializeField] float tweenTime;

    public GameObject buttonStart;

    float dragThreshould;

    private void Awake()
    {
        currentPage = 1;
        targetPos = levelPagesRect.localPosition;
        dragThreshould = Screen.width / 15;
    }

    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            StartCoroutine(MovePage());
        }
    }

    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            StartCoroutine(MovePage());
        }
    }

    IEnumerator MovePage()
    {
        Vector3 startPos = levelPagesRect.localPosition;
        float elapsedTime = 0f; 

        while (elapsedTime < tweenTime)
        {
            levelPagesRect.localPosition = Vector3.Lerp(startPos, targetPos, elapsedTime / tweenTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        levelPagesRect.localPosition = targetPos;
        CheckIsBuyRecord();
    }

    private void Start()
    {
        if (Data.dataInstance.mainMenu.records[0].isBuy == 0)
        {
            buttonStart.SetActive(false);
        }
        else
        {
            buttonStart.SetActive(true);
        }
    }

    public void CheckIsBuyRecord()
    {
        int count = currentPage;
        count--;

        if (Data.dataInstance.mainMenu.records[count].isBuy == 0)
        {
            buttonStart.SetActive(false);
        }
        else
        {
            buttonStart.SetActive(true);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshould)
        {
            if (eventData.position.x > eventData.pressPosition.x) Previous();
            else Next();
        }
        else
        {
            StartCoroutine(MovePage());
        }
    }
}