using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placementLayermask;

    public event Action OnClicked, OnExit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Hit: " + lastPosition);
            OnClicked?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectedMapPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100, placementLayermask);
            if (hit)
            {
                lastPosition = hit.point;
                // Debug.Log("Hit: " + lastPosition);
            }
        return lastPosition;
    }
}
