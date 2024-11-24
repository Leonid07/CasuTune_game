using Anim;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Record : MonoBehaviour
{
    // animation
    public RectTransform record;
    public float speedRotate = 100;

    // music
    public Sounds sounds;
    public AudioSource audioSource;

    //star
    public GameObject parentStarts;
    public List<Image> star;

    // куплена или нет
    public int isBuy = 0;// 0 не куплена 1 куплена

    private void Start()
    {
        star = new List<Image>();
        foreach (var image in parentStarts.GetComponentsInChildren<Image>())
        {
            if (image.gameObject != parentStarts)
            {
                star.Add(image);
            }
        }

        StartCoroutine(UtillsAnim.RotateImageAnim(record, speedRotate));
    }

    public void StopCoroutineAnimaRecord()
    {
        StopCoroutine(UtillsAnim.RotateImageAnim(record, speedRotate));
    }
    public void StartCoroutineAnimaRecord()
    {
        StartCoroutine(UtillsAnim.RotateImageAnim(record, speedRotate));
    }
}
