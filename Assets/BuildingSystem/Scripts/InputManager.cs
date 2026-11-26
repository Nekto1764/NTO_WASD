using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private LayerMask placementLayerMask;

    [SerializeField]  private Vector3 lastPosition;

    private Camera sceneCamera;

    public event Action OnClicked, OnExit;

    private void Start()
    {
        sceneCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
             OnClicked?.Invoke();
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0))
             OnExit?.Invoke();
    }

    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();


    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementLayerMask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }
}
