using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class FoodManager : MonoBehaviour
{
    public GameObject foodObject;

    private List<GameObject> foods = new List<GameObject>();

    public float spawnYPos = 10f;
    public float rayDistance = 10f;
    public LayerMask layerMask;

    private bool isFoodSelected = false;

    private static FoodManager instance;
    public static FoodManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    public void SelectedFoodUI()
    {
        isFoodSelected = !isFoodSelected;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && isFoodSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, rayDistance, layerMask);
            if (hit)
            {
                Debug.Log("Hit: " + hit.collider.name + " : " + hit.point);
                Debug.DrawLine(ray.origin, hit.point, Color.red, 2f);

                FoodSpawn(hit.point);
            }
        }
#endif
    }

    private void FoodSpawn(Vector2 pos)
    {
        pos.y = spawnYPos;
        foods.Add(Instantiate(foodObject, pos, Quaternion.identity));
    }

    public GameObject FindFood(Vector2 target)
    {
        if (foods.Count == 0)
            return null;

        return foods.OrderBy(x => Vector2.Distance(x.transform.position, target)).FirstOrDefault();
    }

    public void RemoveFood(GameObject food)
    {
        foods.Remove(food);
    }
}
