
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public Transform target;

    public float maxSpeed = 3.0f;
    public float acceleration = 2.0f;

    private float currentSpeed = 0.0f;
    private Vector2 movementDirection = Vector2.zero;

    private float idleTimer = 0.0f;

    private bool isAcceleration = true;
    private bool isIdle = false;
    private bool hasFlipped = false;

    private Rigidbody2D rigid2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Vector2 newPos = Random.insideUnitCircle * 5f;
        target.position = newPos;
        SetTarget(target);
    }

    private void Update()
    {
        if (isIdle)
        {
            IdleState();
        }
        else
        {
            Movement();
        }
    }

    private void Movement()
    {
        if (isAcceleration && !IsPassTarget())
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        }
        else
        {
            if (currentSpeed > 0)
            {
                float decelerationFactor = IsPassTarget() ? DecelerationFactor() : 1;
                currentSpeed -= acceleration * decelerationFactor * Time.deltaTime;
                if (currentSpeed <= 0 && !isIdle)
                {
                    currentSpeed = 0;
                    Idle();
                }
            }
            else if (!isIdle)
            {
                Idle();
            }
        }

        rigid2D.velocity = movementDirection * currentSpeed;
    }

    private void Idle()
    {
        isIdle = true;
        idleTimer = Random.Range(0, 1);
        rigid2D.velocity = Vector2.zero;
    }

    private bool IsPassTarget()
    {
        if (target == null)
            return false;

        Vector2 toTarget = (target.position - transform.position).normalized;
        Vector2 currentDirection = rigid2D.velocity.normalized;

        return Vector2.Dot(toTarget, currentDirection) < 0; //ถ้าผ่านค่าจะต้องเป็นลบ -1
    }

    private float DecelerationFactor()
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);// ระยะห่างระหว่างตัวเองกับเป้าหมาย 10 เมตร
        float factor = 1f - Mathf.Clamp(distanceToTarget / 5f, 0f, 1f); //เราสนใจแค่ 2 เมตร ถ้าเกิน 2 เมตร จะเป็น 1 ถ้าน้อยกว่า 2 เมตร จะเป็น 0
        return Mathf.Lerp(0.1f, 1.5f, factor);
    }

    private void IdleState()
    {
        if (idleTimer > 0)
        {
            idleTimer -= Time.deltaTime;
        }
        else
        {
            isIdle = false;

            if (target != null)
            {
                Vector2 newPos = Random.insideUnitCircle * 7f;
                target.position = newPos;
                SetTarget(target);
            }
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        MovementDirection();
        currentSpeed = 0.0f;
        isAcceleration = true;
        isIdle = false;
    }

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
}
