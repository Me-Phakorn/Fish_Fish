using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    public PlaceableObjectData placeableObjectData;

    public void SelectItem()
    {
        GridPlacement gridPlacement = FindObjectOfType<GridPlacement>();
        gridPlacement.SetPlaceObject(placeableObjectData);
    }
}
