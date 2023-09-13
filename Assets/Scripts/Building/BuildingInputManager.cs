using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingInputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    [SerializeField] LayerMask placementLayermask;
    private Vector3 lastPosition;

    public event Action onClicked, onExit;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            onClicked?.Invoke();
        if(Input.GetKeyDown(KeyCode.Escape))
            onExit?.Invoke();
    }

    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out var hit, 100, placementLayermask))
        {
            lastPosition = hit.point;
        }
            
        return lastPosition;
    }
}
