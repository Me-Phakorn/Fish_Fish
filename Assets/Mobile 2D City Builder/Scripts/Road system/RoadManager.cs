using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileCityBuilder
{
    public class RoadManager : MonoBehaviour
    {
        [HideInInspector]
        public GameObject selectedObject;
        public GameObject roadPrefab;

        [HideInInspector]
        public List<RoadThatWasRecentlyMadeData> roadsThatWasRecentlyMadeArray = new List<RoadThatWasRecentlyMadeData>();

        [HideInInspector]
        public bool changeTheSprite;
        [HideInInspector]
        public bool buildRoadsByClick = false;
        
        void Update()
        {
            if(Input.GetMouseButtonDown(0) && buildRoadsByClick)
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
                if(hit.collider == null)
                {
                    changeTheSprite = true;
                    GameObject road = (GameObject)Instantiate(roadPrefab, worldPoint, Quaternion.identity);
                    Road builtRoad = road.GetComponent<Road>();
                    builtRoad.recentlyBuilt = true;
                    selectedObject = road;  
                }
            }
        }

        public void AddingInfoFromRoadsArray()
        {
            for(int i = 0; i < roadsThatWasRecentlyMadeArray.Count; i++)
            {
                GameObject road = roadsThatWasRecentlyMadeArray[i].roadGameObject;
                Road roadType = road.GetComponent<Road>();

                roadType.AddingInfoToSaveArray();
            }
            roadsThatWasRecentlyMadeArray.Clear();
        }
    }
}
