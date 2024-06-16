using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MobileCityBuilder
{
    public class Road : MonoBehaviour
    {
        public BuildingType buildingType;
        [HideInInspector]
        public BuildingData buildingData;
        
        public Collider2D triggerPointUp, triggerPointRight, triggerPointBottom, triggerPointLeft;
        public LayerMask roadLayer;
        [HideInInspector]
        public bool recentlyBuilt;
        [HideInInspector]
        public RoadThatWasRecentlyMadeData roadData;
        private bool up, right, bottom, left;

        private SpriteRenderer background;
        private RoadManager roadManager;
        private GameManager gameManager;

        [System.Serializable]
        public class RoadType
        {
            public Sprite roadTypeSprite;
            public bool up, right, bottom, left;
        }
        public RoadType[] roadTypes;

        void Start()
        {
            roadManager = GameObject.Find("RoadManager").GetComponent<RoadManager>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            background = GetComponent<SpriteRenderer>();
            GetRoadType();
            if(recentlyBuilt)
            {
                roadData.roadGameObject = gameObject;
                roadData.roadPosition = transform.position;
                roadData.roadRotation = transform.rotation;
                roadManager.roadsThatWasRecentlyMadeArray.Add(roadData);
                recentlyBuilt = false;
            }
        }

        void Update()
        {
           if(roadManager.changeTheSprite == true)
           {
                GetRoadType();
           }
        }

        public void AddingInfoToSaveArray()
        {

            buildingData.buildingType = buildingType;
            buildingData.position = transform.position;
            buildingData.rotation = transform.rotation;
            gameManager.save.buildings.Add(buildingData);
            
        }

        public void GetRoadType()
        {
            if(triggerPointUp.IsTouchingLayers(roadLayer))
            {
                up = true;
            }
            else
            {
                up = false;
            }
            if(triggerPointRight.IsTouchingLayers(roadLayer))
            {
                right = true;
            }
            else
            {
                right = false;
            }
            if(triggerPointBottom.IsTouchingLayers(roadLayer))
            {
                bottom = true;
            }
            else
            {
                bottom = false;
            }
            if(triggerPointLeft.IsTouchingLayers(roadLayer))
            {
                left = true; 
            }
            else
            {
                left = false; 
            }

            foreach(RoadType road in roadTypes)
            {
                if(road.up == up && road.right == right && road.bottom == bottom && road.left == left)
                {
                    background.sprite = road.roadTypeSprite;
                }
            }
        }
    }
}

