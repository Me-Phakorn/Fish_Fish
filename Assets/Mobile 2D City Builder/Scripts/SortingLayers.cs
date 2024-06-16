using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileCityBuilder
{
    public class SortingLayers : MonoBehaviour
    {
        private SpriteRenderer sprite;
        public float number;
        
        void Start()
        {
            sprite = GetComponent<SpriteRenderer>();
        }
        
        void Update()
        {
            sprite.sortingOrder = (int)((transform.position.y - number) * (-100));
        }
    }
}
