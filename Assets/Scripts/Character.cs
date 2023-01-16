using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : AnimatedObject
{
    public Rigidbody Rigidbody { get; set; }

    public CharacterDataSO GolemDataObject;
    public GameObject CharacterMeshObject;

    private CharacterStateMachine stateMachine;

    public override Dictionary<ObjectStates, AnimationClip> AnimationValuePairs { get; set; }

    protected Transform attackSpawnLocation;

    [SerializeField] private float groundCheckDistance; 
    private float JumpDurationStartTime;
    public float jumpDuration;
    private float attackCooldownTimer;
    private Vector3 velocityBeforePhysicsUpdate;
    private int groundExplosionsCounter;
    public bool canJump;
    public bool currentlyJumping;
    private bool isMoving;
    private bool isCurrentGolem;
    public bool isGrounded;
    public bool isAttacking;
    protected bool isMovingRight;
    private RaycastHit ray;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        stateMachine = GetComponent<CharacterStateMachine>();
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
            if (!isGrounded)
            {
                GroundChecker();
            }    
        }
        
    }

    private void FixedUpdate()
    {
        velocityBeforePhysicsUpdate = Rigidbody.velocity;
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
        if (!isMoving || !isGrounded)
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

    public void GroundChecker()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out ray, groundCheckDistance)) 
        {
            isGrounded = true;
            canJump = true;
            jumpDuration = JumpDurationStartTime;
        }
        else
        {
            isGrounded = false;
        };
        
    }

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
        if (Rigidbody.velocity.x > GolemDataObject.MaxMoveSpeed && input > 0 || Rigidbody.velocity.x < -GolemDataObject.MaxMoveSpeed && input < 0 || !isGrounded)
        {
            Rigidbody.drag = 0;
            return;
        }
        if (input == 0)
        {
            isMoving = false;
            Rigidbody.drag = 0;
            if (isGrounded && !currentlyJumping)
            {
                Rigidbody.drag = 10;
            }           
        }
        Rigidbody.AddForce(new Vector3(input * (GolemDataObject.MoveSpeedMultiplier + stateMachine.Buffs[(int)stateMachine.ActiveBuff].MoveSpeedModifier) * Time.deltaTime, 0, 0));
        if (input != 0)
        {
            Rigidbody.drag = 0;
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
        if (!isJumping)
        {
            currentlyJumping = false;
        }
        if (canJump && jumpDuration > 0.0f && isJumping)
        {
            groundExplosionsCounter = 0;
            Rigidbody.drag = 0;
            Rigidbody.AddForce(new Vector3(0, (GolemDataObject.JumpSpeed + stateMachine.Buffs[(int)stateMachine.ActiveBuff].JumpSpeedModifier) * Time.deltaTime, 0));
            jumpDuration -= Time.deltaTime;
            currentlyJumping = true;
            isGrounded = false;
            if (jumpDuration < 0.0f)
            {
                canJump = false;
                currentlyJumping = false;
                
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (velocityBeforePhysicsUpdate.y < -10f && stateMachine.ActiveBuff == CharacterStateMachine.GolemBuffs.CrushingLanding && groundExplosionsCounter == 0)
        {
            groundExplosionsCounter++;
            Debug.Log(velocityBeforePhysicsUpdate.y);
            GameObject clone = Instantiate(GetComponent<CharacterGreyGolem>().GolemDataObject.AttackProjectileObject, new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), Quaternion.identity);
            EventManager.Instance.ParticlePlayEvent(stateMachine.ActiveBuffEffect, new Vector3(transform.position.x, transform.position.y - 1, transform.position.z));
        }
        isGrounded = true;
        canJump = true;
        jumpDuration = JumpDurationStartTime;
    }
}
