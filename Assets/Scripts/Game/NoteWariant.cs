using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoteWariant : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    public Direction direction;
    Animator animator;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private bool visible;
    public bool Visible
    {
        get => visible;
        set
        {
            visible = value;
            if (!visible) animator.Play("Invisible");
        }
    }

    public bool Played { get; set; }
    public int Id { get; set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private Vector2 startDragPosition;
    public float minSwipeLength = 50f; // Minimum length of swipe in pixels

    public void OnPointerDown(PointerEventData eventData)
    {
        startDragPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 endDragPosition = eventData.position;
        Vector2 swipeDirection = endDragPosition - startDragPosition;

        if (swipeDirection.magnitude >= minSwipeLength)
        {
            Direction detectedDirection = GetSwipeDirection(swipeDirection);
            Play(detectedDirection);
            GameController.Instance.Article(gameObject);
        }
    }

    private Direction GetSwipeDirection(Vector2 swipeDirection)
    {
        if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
        {
            return swipeDirection.x > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            return swipeDirection.y > 0 ? Direction.Up : Direction.Down;
        }
    }

    public void Play(Direction swipeDirection)
    {
        if (direction == swipeDirection)
        {
            if (Visible)
            {
                if (!Played && GameController.Instance.LastPlayedNoteId == Id - 1)
                {
                    Played = true;
                    GameController.Instance.LastPlayedNoteId = Id;
                    GameController.Instance.ChangeSlider(GameController.Instance.gameOverSlider, false, GameController.Instance.starImageGameOver, GameController.Instance.scoreGameOver);// счёт
                    GameController.Instance.ChangeSlider(GameController.Instance.sliderScore, false, GameController.Instance.starImage, GameController.Instance.textScore);// счёт
                    animator.Play("Played");

                    GameObject parentObject = transform.parent.gameObject;
                    SpriteRenderer transparentImage = null;

                    for (int i = parentObject.transform.childCount - 1; i >= 0; i--)
                    {
                        Transform child = parentObject.transform.GetChild(i);
                        SpriteRenderer image = child.GetComponent<SpriteRenderer>();
                        if (image != null && image.color.a == 1)
                        {
                            transparentImage = image;
                            break;
                        }
                    }

                    if (transparentImage != null && transparentImage.gameObject.GetInstanceID() == gameObject.GetInstanceID())
                    {
                        GameController.Instance.Over.Over();
                        GameController.Instance.WictoryPanel.SetActive(true);
                        GameController.Instance.panelWictory.textScore.text = $"SCORE: {GameController.Instance.sliderScore.value}";
                        GameController.Instance.CountStarInSlider();
                    }
                }
            }
            else
            {
                Played = true;
                GameController.Instance.LastPlayedNoteId = Id;
                animator.Play("Played");

                GameController.Instance.EndGame();
                GameController.Instance.Over.Over();
                GameController.Instance.Over.StartCouroutine();
                animator.Play("Missed");
            }
        }
    }

    public void OutOfScreen()
    {
        if (Visible && !Played)
        {
            Played = true;
            GameController.Instance.LastPlayedNoteId = Id;
            animator.Play("Played");

            GameController.Instance.EndGame();
            GameController.Instance.Over.StartCouroutine();
            GameController.Instance.Over.Over();
            animator.Play("Missed");
        }
    }
}