using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : AnimatedObject
{
    public Rigidbody Rigidbody { get; set; }

    public CharacterDataSO GolemDataObject;
    public GameObject CharacterMeshObject;
    public override Dictionary<ObjectStates, AnimationClip> AnimationValuePairs { get; set; }

    private float JumpDurationStartTime;
    private float jumpDuration;
    private bool canJump;
    private bool isMoving;
    private bool isCurrentGolem;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        CharacterMeshObject.gameObject.SetActive(false);
        AnimationValuePairs = new Dictionary<ObjectStates, AnimationClip>()
        {
            {ObjectStates.Idle, GolemDataObject.IdleAnim },
            {ObjectStates.Walking, GolemDataObject.WalkAnim }
        };
        JumpDurationStartTime = GolemDataObject.JumpDuration;
        jumpDuration = GolemDataObject.JumpDuration;
    }

    private void Update()
    {
        if (isCurrentGolem)
        {
            UpdateAnimationState();
            CheckAnimationState();
        }     
    }

    public void InitializeGolem()
    {
        EventManager.Instance.EventMoveInput.AddListener(Move);
        EventManager.Instance.EventJumpInput.AddListener(Jump);      
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
        CharacterMeshObject.gameObject.SetActive(false);
        isCurrentGolem = false;
    }

    public void UpdateAnimationState()
    {
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

    public virtual void Move(Vector2 input)
    {
        if (Rigidbody.velocity.x > GolemDataObject.MaxMoveSpeed && input.x > 0 || Rigidbody.velocity.x < -GolemDataObject.MaxMoveSpeed && input.x < 0)
        {
            return;
        }
        Rigidbody.AddForce(new Vector3(input.x * GolemDataObject.MoveSpeedMultiplier, 0, 0));
        if (input.x != 0)
        {
            isMoving = true;
            RotateGolem(input.x);
        }
        else
        {
            isMoving = false;
        }
    }

    private void RotateGolem(float xInput)
    {
        if (xInput >= 0)
        {
            CharacterMeshObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            CharacterMeshObject.transform.rotation = Quaternion.Euler(0, 270, 0);
        }
    }

    public virtual void Jump(bool isJumping)
    {
        if (canJump && jumpDuration > 0.0f && isJumping)
        {
            Rigidbody.AddForce(new Vector3(0, GolemDataObject.JumpSpeed, 0));
            jumpDuration -= Time.deltaTime;
            Debug.Log("Jumping");
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
