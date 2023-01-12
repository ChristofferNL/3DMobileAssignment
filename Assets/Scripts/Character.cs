using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : AnimatedObject
{
    public Rigidbody Rigidbody { get; set; }

    public CharacterDataSO GolemDataObject;
    public GameObject CharacterMeshObject;

    public override Dictionary<ObjectStates, AnimationClip> AnimationValuePairs { get; set; }

    protected Transform attackSpawnLocation;

    private float JumpDurationStartTime;
    private float jumpDuration;
    private float attackCooldownTimer;
    private bool canJump;
    private bool isMoving;
    private bool isCurrentGolem;
    public bool isAttacking;
    protected bool isMovingRight;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        CharacterMeshObject.gameObject.SetActive(false);
        AnimationValuePairs = new Dictionary<ObjectStates, AnimationClip>()
        {
            {ObjectStates.Idle, GolemDataObject.IdleAnim },
            {ObjectStates.Walking, GolemDataObject.WalkAnim },
            {ObjectStates.AttackOne, GolemDataObject.AttackAnim }
        };
        JumpDurationStartTime = GolemDataObject.JumpDuration;
        jumpDuration = GolemDataObject.JumpDuration;
        attackCooldownTimer = GolemDataObject.AttackCooldown;
        attackSpawnLocation = GameObject.Find("AttackBoxLocation").transform;
        isMovingRight = true;
    }

    private void Update()
    {
        if (isCurrentGolem)
        {
            UpdateAnimationState();
            CheckAnimationState();
            AttackResetMethod();
        }
    }

    public void InitializeGolem()
    {
        EventManager.Instance.EventMoveInput.AddListener(Move);
        EventManager.Instance.EventJumpInput.AddListener(Jump);
        EventManager.Instance.EventAttackInput.AddListener(SpecialAttack);
        CharacterMeshObject.gameObject.SetActive(true);
        isCurrentGolem = true;
        if (Rigidbody.velocity.y != 0)
        {
            canJump = false;
        }
    }

    public void UnInitializeGolem()
    {
        EventManager.Instance.EventMoveInput.RemoveListener(Move);
        EventManager.Instance.EventJumpInput.RemoveListener(Jump);
        EventManager.Instance.EventAttackInput.RemoveListener(SpecialAttack);
        CharacterMeshObject.gameObject.SetActive(false);
        isCurrentGolem = false;
    }

    public void UpdateAnimationState()
    {
        if (isAttacking)
        {
            ActiveState = ObjectStates.AttackOne;
            return;
        }
        if (!isMoving)
        {
            ActiveState = ObjectStates.Idle;
            return;
        }
        if (isMoving)
        {
            ActiveState = ObjectStates.Walking;
            return;
        }
    }

    public abstract void CreateAttackHitbox();


    public void AttackResetMethod()
    {
        if (isAttacking)
        {
            attackCooldownTimer -= Time.deltaTime;
            if (attackCooldownTimer <= 0.0f)
            {
                CreateAttackHitbox();
                isAttacking = false;
                attackCooldownTimer = GolemDataObject.AttackCooldown;
                ActiveState = ObjectStates.Idle;
                CheckAnimationState();
            }
        }
    }

    public void SpecialAttack(bool value)
    {
        if (value && attackCooldownTimer == GolemDataObject.AttackCooldown)
        {
            isAttacking = true;
        }
    }

    public virtual void Move(float input)
    {
        if (Rigidbody.velocity.x > GolemDataObject.MaxMoveSpeed && input > 0 || Rigidbody.velocity.x < -GolemDataObject.MaxMoveSpeed && input < 0)
        {
            return;
        }
        if (input == 0)
        {
            isMoving = false;
        }
        Rigidbody.AddForce(new Vector3(input * GolemDataObject.MoveSpeedMultiplier * Time.deltaTime, 0, 0));
        if (input != 0)
        {
            isMoving = true;
            RotateGolem(input);
        }
    }

    private void RotateGolem(float xInput)
    {
        if (xInput >= 0)
        {
            CharacterMeshObject.transform.rotation = Quaternion.Euler(0, 90, 0);
            isMovingRight = true;
        }
        else
        {
            CharacterMeshObject.transform.rotation = Quaternion.Euler(0, 270, 0);
            isMovingRight = false;
        }
    }

    public virtual void Jump(bool isJumping)
    {
        if (canJump && jumpDuration > 0.0f && isJumping)
        {
            Rigidbody.AddForce(new Vector3(0, GolemDataObject.JumpSpeed * Time.deltaTime, 0));
            jumpDuration -= Time.deltaTime;
            if (jumpDuration < 0.0f)
            {
                canJump = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        canJump = true;
        jumpDuration = JumpDurationStartTime;
    }
}
