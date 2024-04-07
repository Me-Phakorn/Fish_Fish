using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTank : MonoBehaviour
{
    [SerializeField]
    private Transform top, bottom, left, right;

    public void Waypoint(){

    }

    private void CalculateInBounds(){
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, left.position.x, right.position.x);
        pos.y = Mathf.Clamp(pos.y, bottom.position.y, top.position.y);
        transform.position = pos;
    }
}
