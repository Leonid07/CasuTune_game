using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    Animator animator;

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

    public void Play()
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
            GameController.Instance.EndGame();
            GameController.Instance.Over.Over();
            GameController.Instance.Over.StartCouroutine();
            animator.Play("Missed");
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
            GameController.Instance.Over.Over();
            GameController.Instance.Over.StartCouroutine();
            animator.Play("Missed");
        }
    }
}