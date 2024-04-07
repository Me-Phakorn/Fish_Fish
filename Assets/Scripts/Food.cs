using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public float foodValue = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fish"))
        {
            Fish fish = other.GetComponent<Fish>();
            fish.fishStat.Hungry += foodValue;

            if (fish.fishStat.Hungry > fish.fishStat.MaxHungry)
                fish.fishStat.Hungry = fish.fishStat.MaxHungry;

            Destroy(gameObject);
        }
        else if (other.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        FoodManager.Instance.RemoveFood(gameObject);
    }
}
