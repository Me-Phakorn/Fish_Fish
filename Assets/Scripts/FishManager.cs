using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP.JSON.LitJson;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public FishDatabase fishDatabase;

    public TextAsset fishDatabaseJson; // from API
    public TextAsset userDataJson; // from API

    private void Start()
    {
        JsonReader reader = new JsonReader(fishDatabaseJson.text);
        JsonData fishData = JsonMapper.ToObject(reader);

        Debug.Log(JsonMapper.ToJson(fishData["fishDatabase"]));

        fishDatabase.fishList.Clear();
        foreach (JsonData fish in fishData["fishDatabase"])
        {
            string imagePath = fish["fishImage"].ToString();
            Sprite image = Resources.Load<Sprite>(imagePath);

            Fish newFish = new Fish(){
                fishName = fish["fishName"].ToString(),
                fishSprite = image,
                description = fish["description"].ToString(),
                fishPrice = int.Parse(fish["fishPrice"].ToString()),
                maxHealth = float.Parse(fish["maxHealth"].ToString()),
                maxHunger = float.Parse(fish["maxHunger"].ToString()),
                maxStamina = float.Parse(fish["maxStamina"].ToString()),
                attack = float.Parse(fish["attack"].ToString()),
                defense = float.Parse(fish["defense"].ToString()),
                speed = float.Parse(fish["speed"].ToString())
            };
            fishDatabase.fishList.Add(newFish);
        }
    }
}
