using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fish Database", menuName = "Fish/Fish Database")]
public class FishDatabase : ScriptableObject
{
    public List<FishInfo> fishList = new List<FishInfo>();
}
