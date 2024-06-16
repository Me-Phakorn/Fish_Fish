using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Fish Setting")]
    public BossStat bossStat;

    [Header("Movement Setting")]
    public Transform target;
    private Transform fishTarget;

    public float speed = 2.0f;

    public IState<Boss> currentState;

    private Vector2 movementDirection;

    private float currentSpeed;
    private float idleTime = 0f;
    private float idleTimer = 0f;

    private bool isDead = false;
    private bool isFlipping = false;
    private bool hasFlipped = false;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid2D;
    private Animator animator;

    private CircleCollider2D circleCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void Initialize()
    {
        bossStat.Initialize();
    }

    private void Start()
    {
        currentSpeed = speed;

        RandomIdleTime();
        ChangeState(new BossIdle());
    }

    public void ChangeState(IState<Boss> newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter(this);
    }

    private void Update()
    {
        if (isDead)
            return;

        currentState?.Execute();
    }

    public void RandomIdleTime()
    {
        currentSpeed = speed;
        idleTime = Random.Range(0.1f, 1f);
    }

    public void SetTarget(Transform target)
    {
        this.target.position = target.position;
        ChangeState(new BossMovement());
    }

    public void Idle()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleTime)
        {
            idleTimer = 0;
            
            Fish[] fish = FindObjectsOfType<Fish>();
            if(fish.Length > 0)
            {
                int randomFish = Random.Range(0, fish.Length);

                if(!fish[randomFish].IsDead)
                {
                    fishTarget = fish[randomFish].transform;
                    SetTarget(fishTarget);
                }
            }
        }
    }

    public void Movement()
    {
        if (target == null || fishTarget == null)
        {
            ChangeState(new BossIdle());
            return;
        }

        target.position = fishTarget.position;

        CheckFlipAnimation();
        rigid2D.MovePosition(rigid2D.position + movementDirection * currentSpeed * Time.fixedDeltaTime); //ติดไว้ก่อน

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            ChangeState(new BossIdle());
        }
    }

    public void Dead()
    {
        rigid2D.gravityScale = .5f;
        spriteRenderer.flipY = true;
        circleCollider.isTrigger = false;
    }

    private void CheckFlipAnimation()
    {
        if (target == null)
            return;

        movementDirection = (target.position - transform.position).normalized;
        if (!isFlipping && (movementDirection.x > 0 && !hasFlipped || movementDirection.x < 0 && hasFlipped))
            animator.SetTrigger("Switch");
    }

    private void OnStartFlip()
    {
        isFlipping = true;
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

        isFlipping = false;
    }
}
