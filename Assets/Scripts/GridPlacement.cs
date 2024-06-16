using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GridPlacement : MonoBehaviour
{
    public Grid grid;
    public Tilemap[] layers;

    [HideInInspector] public Tilemap previewLayer;
    [HideInInspector] public TileBase previewTile;

    [Header("Preview Settings")]
    public Color previewColor = Color.green;
    public Color previewInvalidColor = Color.red;

    public PreviewUI previewUI;
    public GameObject itemsUI;

    public LayerMask itemLayerMask;

    private Vector3Int? currentPreviewPosition = null;
    private PlaceableObjectData currentPlaceableObject;

    private bool isPreviewing = false;
    private bool isDragging = false;
    private bool isEditing = false;

    private GameObject previewObject;

    private Dictionary<int, HashSet<Vector3Int>> placedPositions = new Dictionary<int, HashSet<Vector3Int>>();
    private GameObject selectedObject;
    private Vector3Int originalPosition;

    public bool CanPlaceObject(Vector3Int startPosition, Vector2Int size, int layerIndex)
    {
        if (!placedPositions.ContainsKey(layerIndex))
        {
            placedPositions[layerIndex] = new HashSet<Vector3Int>();
        }

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3Int position = new Vector3Int(startPosition.x + x, startPosition.y + y, startPosition.z);
                if (placedPositions[layerIndex].Contains(position))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void SetPlaceObject(PlaceableObjectData placeableObject)
    {
        ClearPreview(false);
        currentPlaceableObject = placeableObject;
        isPreviewing = true;
        isEditing = false;

        previewLayer = layers[placeableObject.layerIndex];

        if (previewObject == null)
        {
            previewObject = Instantiate(placeableObject.prefab);
            previewObject.name = "PreviewObject";
        }
        else
        {
            Destroy(previewObject);
            previewObject = Instantiate(placeableObject.prefab);
            previewObject.name = "PreviewObject";
        }

        SpriteRenderer[] renderers = previewObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.color = previewColor;
        }

        previewUI.SetTarget(previewObject.transform);
        previewUI.Show();
        itemsUI.SetActive(false);
    }

    public void SelectObject(GameObject obj)
    {
        var placeableObjectComponent = obj.GetComponent<PlaceableObjectComponent>();
        if (placeableObjectComponent == null) return;

        selectedObject = obj;
        originalPosition = grid.WorldToCell(obj.transform.position);
        currentPlaceableObject = placeableObjectComponent.placeableObjectData;
        isEditing = true;
        isPreviewing = true;

        previewLayer = layers[currentPlaceableObject.layerIndex];

        if (previewObject == null)
        {
            previewObject = Instantiate(obj);
            previewObject.name = "PreviewObject";
        }
        else
        {
            Destroy(previewObject);
            previewObject = Instantiate(obj);
            previewObject.name = "PreviewObject";
        }

        previewObject.transform.position = obj.transform.position;

        SpriteRenderer[] renderers = previewObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.color = previewColor;
        }

        previewUI.SetTarget(previewObject.transform);
        previewUI.Show();
        itemsUI.SetActive(false);
    }

    public void PreviewObject(Vector3Int startPosition, PlaceableObjectData placeableObject)
    {
        ClearTiles();

        bool canPlace = CanPlaceObject(startPosition, placeableObject.size, placeableObject.layerIndex);
        currentPreviewPosition = startPosition;

        Color tileColor = canPlace ? this.previewColor : this.previewInvalidColor;

        for (int x = 0; x < placeableObject.size.x; x++)
        {
            for (int y = 0; y < placeableObject.size.y; y++)
            {
                Vector3Int position = new Vector3Int(startPosition.x + x, startPosition.y + y, startPosition.z);
                previewLayer.SetTile(position, previewTile);
                previewLayer.SetColor(position, tileColor);
            }
        }

        previewObject.transform.position = grid.CellToWorld(startPosition) + grid.cellSize / 2;

        SpriteRenderer[] renderers = previewObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.color = tileColor;
        }
    }

    public void ConfirmPlacement()
    {
        if (currentPreviewPosition == null || currentPlaceableObject == null) return;

        Vector3Int startPosition = currentPreviewPosition.Value;
        PlaceableObjectData placeableObject = currentPlaceableObject;

        if (!CanPlaceObject(startPosition, placeableObject.size, placeableObject.layerIndex)) return;

        Tilemap layer = layers[placeableObject.layerIndex];

        if (isEditing)
        {
            // Remove the original position from placedPositions
            for (int x = 0; x < placeableObject.size.x; x++)
            {
                for (int y = 0; y < placeableObject.size.y; y++)
                {
                    Vector3Int position = new Vector3Int(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
                    placedPositions[placeableObject.layerIndex].Remove(position);
                }
            }

            Destroy(selectedObject);
            isEditing = false;
        }

        for (int x = 0; x < placeableObject.size.x; x++)
        {
            for (int y = 0; y < placeableObject.size.y; y++)
            {
                Vector3Int position = new Vector3Int(startPosition.x + x, startPosition.y + y, startPosition.z);
                layer.SetTile(position, placeableObject.tile);
                placedPositions[placeableObject.layerIndex].Add(position);
            }
        }

        GameObject placedObject = Instantiate(placeableObject.prefab);
        placedObject.transform.position = grid.CellToWorld(startPosition) + grid.cellSize / 2;

        ClearPreview(true);
        isPreviewing = false;
        itemsUI.SetActive(true);
    }

    public void CancelEditing()
    {
        if (!isEditing) return;

        isEditing = false;
        isPreviewing = false;
        ClearPreview(true);

        // Revert the preview object to its original position
        GameObject placedObject = Instantiate(currentPlaceableObject.prefab);
        placedObject.transform.position = grid.CellToWorld(originalPosition) + grid.cellSize / 2;
        placedObject.GetComponent<PlaceableObjectComponent>().placeableObjectData = currentPlaceableObject;

        // Add the positions back to placedPositions
        for (int x = 0; x < currentPlaceableObject.size.x; x++)
        {
            for (int y = 0; y < currentPlaceableObject.size.y; y++)
            {
                Vector3Int position = new Vector3Int(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
                placedPositions[currentPlaceableObject.layerIndex].Add(position);
            }
        }

        itemsUI.SetActive(true);
    }

    public void ClearTiles()
    {
        if (currentPreviewPosition == null) return;

        Vector3Int startPosition = currentPreviewPosition.Value;
        Vector2Int size = currentPlaceableObject.size;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3Int position = new Vector3Int(startPosition.x + x, startPosition.y + y, startPosition.z);
                previewLayer.SetTile(position, null);
            }
        }

        currentPreviewPosition = null;
    }

    public void ClearPreview(bool destroyObject = true)
    {
        ClearTiles();

        if (destroyObject && previewObject != null)
        {
            Destroy(previewObject);
        }

        previewUI.Hide();
        itemsUI.SetActive(true);
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (isEditing)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelEditing();
            }
        }

        if (isPreviewing)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = grid.WorldToCell(mouseWorldPos);

            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            if (isDragging)
            {
                PreviewObject(cellPosition, currentPlaceableObject);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, Mathf.Infinity, itemLayerMask);
                if (hit.collider != null)
                {
                    SelectObject(hit.collider.gameObject);
                }
            }
        }
    }
}
