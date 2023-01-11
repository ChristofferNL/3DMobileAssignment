using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : AnimatedObject
{
    [HideInInspector] public Rigidbody Rigidbody;
    public List<GameObject> CharacterMesh;
    public float MoveSpeedMultiplier;
    public float MaxMoveSpeed;
    public float JumpDuration;
    public float JumpSpeed;
    public bool canJump;
    public bool IsMoving;
    protected float JumpDurationStartTime;
    private int currentGolemType;

    public override Animator Animator { get; set; }
    public override Dictionary<ObjectStates, AnimationClip> AnimationValuePairs { get; set; }

    public AnimationClip IdleAnim;
    public AnimationClip WalkAnim;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        EventManager.Instance.EventMoveInput.AddListener(Move);
        EventManager.Instance.EventJumpInput.AddListener(Jump);
        UIManager.Instance.EventChangeGolem.AddListener(ChangeGolemType);
        JumpDurationStartTime = JumpDuration;
        AnimationValuePairs = new Dictionary<ObjectStates, AnimationClip>()
        {
            {ObjectStates.Idle, IdleAnim },
            {ObjectStates.Walking, WalkAnim }
        };
    }

    private void Update()
    {
        UpdateAnimationState();
        CheckAnimationState();
    }

    public void ChangeGolemType(CharacterDataSO dataSO)
    {
        MoveSpeedMultiplier = dataSO.MoveSpeedMultiplier;
        MaxMoveSpeed = dataSO.MaxMoveSpeed;
        JumpDuration = dataSO.JumpDuration;
        JumpSpeed = dataSO.JumpSpeed;
    }

    public void UpdateAnimationState()
    {
        if (!IsMoving)
        {
            ActiveState = ObjectStates.Idle;
            return;
        }
        if (IsMoving)
        {
            ActiveState = ObjectStates.Walking;
            return;
        }
    }

    public virtual void Move(Vector2 input)
    {
        if (Rigidbody.velocity.x > MaxMoveSpeed && input.x > 0 || Rigidbody.velocity.x < -MaxMoveSpeed && input.x < 0)
        {
            return;
        }
        Rigidbody.AddForce(new Vector3(input.x * MoveSpeedMultiplier, 0, 0));
        if (input.x != 0)
        {
            IsMoving = true;
            if (input.x > 0)
            {
                CharacterMesh[currentGolemType].transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                CharacterMesh[currentGolemType].transform.rotation = Quaternion.Euler(0, 270, 0);
            }
        }
        else
        {
            IsMoving = false;
        }
    }

    public virtual void Jump(bool isJumping)
    {
        if (canJump && JumpDuration > 0.0f && isJumping)
        {
            Rigidbody.AddForce(new Vector3(0, JumpSpeed, 0));
            JumpDuration -= Time.deltaTime;
            Debug.Log("Jumping");
            if (JumpDuration < 0.0f)
            {
                canJump = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        canJump = true;
        JumpDuration = JumpDurationStartTime;
    }
}
