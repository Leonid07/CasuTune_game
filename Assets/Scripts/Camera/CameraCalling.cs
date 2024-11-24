using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCalling : MonoBehaviour
{
    public Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        CheckVisibility();
    }

    void CheckVisibility()
    {
        SpriteRenderer[] sprites = FindObjectsOfType<SpriteRenderer>();
        foreach (var sprite in sprites)
        {
            Vector3 screenPoint = cam.WorldToViewportPoint(sprite.transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
            sprite.enabled = onScreen;
        }
    }
}
