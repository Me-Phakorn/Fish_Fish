using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP;
using BestHTTP.JSON.LitJson;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public Transform spawnFishPoint;
    public Transform fishTank;

    public GameObject fishPrefab;

    private async void Start()
    {
        HTTPRequest request = new HTTPRequest(new Uri("http://localhost:3000/user/fish"));

        string responseString = await request.GetAsStringAsync();
        JsonData jsonData = JsonMapper.ToObject(responseString);

        Debug.Log(jsonData.ToJson());

        if (!jsonData["fishTask"].IsArray)
        {
            Debug.Log("No fish task");
            return;
        }

        foreach (JsonData fish in jsonData["fishTask"])
        {
            GameObject _fish = (GameObject)Instantiate(fishPrefab, spawnFishPoint.position, Quaternion.identity);
            _fish.GetComponentInChildren<Fish>().Initialize((float)fish["stats"]["hunger"]);

            if((float)fish["stats"]["hunger"] <= 0){
                _fish.transform.position = UnityEngine.Random.insideUnitCircle * 5f;
            }

            Debug.Log(fish["fishName"] + " " + fish["stats"]["hunger"]);
        }
    }
}
