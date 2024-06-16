using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileCityBuilder
{
    public class CameraController : MonoBehaviour
    {
        public bool movement;
        public float movementTime;
        private bool multiTouch;

        public float zoomOutMin = 1;
        public float zoomOutMax = 8;

        [Header("Limits")]
        public float leftLimit;
        public float rightLimit;
        public float bottomLimit;
        public float upperLimit;

        private Vector3 dragStartPosition;
        private Vector3 dragCurrentPosition;
        private Vector3 newPosition;
        private Vector3 target;

        public int moveSpeed;
        public RoadManager roadManager;
        public GameManager gameManager;

        void Start()
        {
            multiTouch = false;
            newPosition = transform.position;
        }

        void Update()
        {
            if(roadManager.buildRoadsByClick)
            {
                if(roadManager.selectedObject != null)
                {
                    target = new Vector3(roadManager.selectedObject.transform.position.x, roadManager.selectedObject.transform.position.y, transform.position.z);
                    Vector3 currentPosition = Vector3.Lerp(transform.position, target, moveSpeed * Time.deltaTime);
                    transform.position = currentPosition;
                }
            }
            else
            {
                HandlerMouseInput();
            }
            
        }

        void HandlerMouseInput()
        {
            if(movement == true)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    multiTouch =  false;

                    Plane plane = new Plane(Vector3.forward, Vector3.zero);

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float entry;
                    if(plane.Raycast(ray, out entry))
                    {
                        dragStartPosition = ray.GetPoint(entry);
                    }
                }
                if(Input.touchCount == 2)
                {
                    multiTouch = true;
                    Touch touchZero = Input.GetTouch(0);
                    Touch touchOne = Input.GetTouch(1);

                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                    float difference = currentMagnitude - prevMagnitude;

                    zoom(difference * 0.003f);
                }
                if(Input.GetMouseButton(0) && multiTouch == false)
                {
                    Plane plane = new Plane(Vector3.forward, Vector3.zero);

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float entry;
                    if(plane.Raycast(ray, out entry))
                    {
                        dragCurrentPosition = ray.GetPoint(entry);

                        newPosition = transform.position + dragStartPosition - dragCurrentPosition;
                    }
                }
                zoom(Input.GetAxis("Mouse ScrollWheel"));
            }
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftLimit, rightLimit), Mathf.Clamp(transform.position.y, bottomLimit, upperLimit), transform.position.z);
            
        }

        void zoom(float increment)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector2(leftLimit, upperLimit), new Vector2(rightLimit, upperLimit));
            Gizmos.DrawLine(new Vector2(leftLimit, bottomLimit), new Vector2(rightLimit, bottomLimit));
            Gizmos.DrawLine(new Vector2(leftLimit, upperLimit), new Vector2(leftLimit, bottomLimit));
            Gizmos.DrawLine(new Vector2(rightLimit, upperLimit), new Vector2(rightLimit, bottomLimit));
        }
    }

}
