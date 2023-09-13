using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    [SerializeField] private BuildingInputManager buildingInputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject gridVisualization;
    [SerializeField] private ObjectDatabaseSo database;
    private int selectedObjectIndex = -1;
    private GridData wallData, objectData;
    private Renderer previewRenderer;
    private List<GameObject> placedGameobject = new List<GameObject>();
    private void Awake()
    {
        StopPlacement();
        wallData = new();
        objectData = new();
        previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }
    private void Update()
    {
        if (selectedObjectIndex < 0)
            return;
        Vector3 mousePosition = buildingInputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        previewRenderer.material.color = placementValidity ? Color.white : Color.red;

        mouseIndicator.transform.position = mousePosition;
        Vector3 cellWorldPosition = grid.CellToWorld(gridPosition);
        cellWorldPosition += Vector3.up * 0.05f; // Dodaj przesunięcie w osi Y
        cellIndicator.transform.position = cellWorldPosition;
    }
    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found{ID}");
        }
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        buildingInputManager.onClicked += PlaceStructure;
        buildingInputManager.onExit += StopPlacement;
    }
    private void PlaceStructure()
    {
        if (buildingInputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = buildingInputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (!placementValidity)
            return;
        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        placedGameobject.Add(newObject);
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 2 ? wallData : objectData;
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].size, database.objectsData[selectedObjectIndex].ID, placedGameobject.Count - 1);
    }
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 2 ? wallData : objectData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].size);
    }
    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        buildingInputManager.onClicked -= PlaceStructure;
        buildingInputManager.onExit -= StopPlacement;
    }
}
