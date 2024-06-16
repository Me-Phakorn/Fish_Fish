using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileCityBuilder
{
    [System.Serializable]
    public class BuildingData
    {
        public BuildingType buildingType;
        public Vector3 position;
        public Quaternion rotation;
        public int Level;
    }

    [System.Serializable]
    public enum BuildingType
    {
        house,
        house2,
        factory,
        powerStation,
        road
    }
}
