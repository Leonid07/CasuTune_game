using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Anim
{
    public readonly struct UtillsAnim
    {

        public static IEnumerator RotateImageAnim(RectTransform uiElement, float rotationSpeed, bool Clockwise = true)
        {
            while (true)
            {
                if (Clockwise)
                    uiElement.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
                else
                    uiElement.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
                yield return null;
            }
        }

        public static IEnumerator MovePanelToPosition(RectTransform uiElement, float duration)
        {
            Vector2 initialPosition = uiElement.anchoredPosition;
            Vector2 targetPosition = new Vector2(0, 0);
            Vector2 initialSize = uiElement.sizeDelta;
            Vector2 targetSize = new Vector2(0, 0);

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);

                uiElement.anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, t);
                uiElement.sizeDelta = Vector2.Lerp(initialSize, targetSize, t);
                yield return null;
            }

            uiElement.anchoredPosition = targetPosition;
            uiElement.sizeDelta = targetSize;
            uiElement.offsetMin = Vector2.zero;
            uiElement.offsetMax = Vector2.zero;

            yield break;
        }
    }
}