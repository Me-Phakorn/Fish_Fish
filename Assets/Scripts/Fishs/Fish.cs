using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("Fish Setting")]
    public FishStat fishStat;

    [Header("Movement Setting")]
    public Transform target;

    public float speed = 2.0f;

    public IState<Fish> currentState;

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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Initialize()
    {

    }

    private void Start()
    {
        fishStat.Initialize();
        currentSpeed = speed;

        RandomIdleTime();
        ChangeState(new FishIdle());
    }

    public void ChangeState(IState<Fish> newState)
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
        fishStat?.Update(Time.deltaTime);

        if (fishStat.Hungry <= 0)
        {
            isDead = true;
            ChangeState(new FishDead());
        }
        else 
        if (fishStat.Hungry <= fishStat.MaxHungry / 3f)
        {
            var _food = FoodManager.Instance.FindFood(transform.position);
            if (_food)
            {
                target.position = _food.transform.position;
                currentSpeed = speed * 2f;
                SetTarget(target);
            }
            else
            {
                currentSpeed = speed;
            }
        }
    }

    public void RandomIdleTime()
    {
        currentSpeed = speed;
        idleTime = Random.Range(0.1f, 1f);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        ChangeState(new FishMovement());
    }

    public void Idle()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleTime)
        {
            idleTimer = 0;
            target.position = Random.insideUnitCircle * 5f;
            SetTarget(target);
        }
    }

    public void Movement()
    {
        if (target == null)
        {
            ChangeState(new FishIdle());
            return;
        }

        CheckFlipAnimation();
        rigid2D.MovePosition(rigid2D.position + movementDirection * currentSpeed * Time.fixedDeltaTime); //ติดไว้ก่อน

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            ChangeState(new FishIdle());
        }
    }

    public void Dead()
    {
        rigid2D.gravityScale = .5f;
        spriteRenderer.flipY = true;
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
