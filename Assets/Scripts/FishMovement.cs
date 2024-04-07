
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FishMovements : MonoBehaviour
{
    public Transform target;

    private Vector2 movementDirection = Vector2.zero;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid2D;
    private Animator animator;

    private bool hasFlipped = false;

    private void MovementDirection()
    {
        if (target == null)
            return;

        movementDirection = (target.position - transform.position).normalized;

        if (movementDirection.x > 0 && !hasFlipped || movementDirection.x < 0 && hasFlipped)
            animator.SetTrigger("Switch");
    }

    private void FlipDirection()
    {
        if (movementDirection.x > 0 && !hasFlipped)
        {
            hasFlipped = true;
        }
        else if (movementDirection.x < 0 && hasFlipped)
        {
            hasFlipped = false;
        }

        spriteRenderer.flipX = hasFlipped;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

    }
}
