using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public float speed = 2.0f;
    public Transform target;

    private Vector2 movementDirection = Vector2.zero;

    private Rigidbody2D rigid2D;

    private void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        movementDirection = (target.position - transform.position).normalized;
        rigid2D.MovePosition(rigid2D.position + movementDirection * speed * Time.fixedDeltaTime);
    }
}
