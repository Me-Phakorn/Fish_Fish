using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewUI : MonoBehaviour
{
    public Canvas canvas;

    private Transform target;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, Camera.main, out Vector2 localPoint);
            transform.localPosition = localPoint;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
