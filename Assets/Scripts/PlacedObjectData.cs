
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlacedObjectData
{
    public int layerIndex;
    public Vector3Int position;
    public string prefabName;
}

[System.Serializable]
public class StageData
{
    public List<PlacedObjectData> placedObjects = new List<PlacedObjectData>();
}
