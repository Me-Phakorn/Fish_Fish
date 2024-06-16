using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "PlaceableObjectData", menuName = "PlaceableObjectData", order = 0)]
public class PlaceableObjectData : ScriptableObject
{
    public string objectName;
    public TileBase tile;
    public Vector2Int size;
    public int layerIndex;
    public GameObject prefab;
}