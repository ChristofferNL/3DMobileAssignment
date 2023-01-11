using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : AnimatedObject
{
    [HideInInspector] public Rigidbody Rigidbody;
    public List<GameObject> CharacterMeshObjects;
    public List<CharacterDataSO> GolemDataObjects = new();
    public float MoveSpeedMultiplier;
    public float MaxMoveSpeed;
    public float JumpDuration;
    public float JumpSpeed;
    public bool canJump;
    public bool IsMoving;
    protected float JumpDurationStartTime;
    private int currentGolemType = 0;

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
        UIManager.Instance.ChangeGolemButton.onClick.AddListener(ChangeGolemType);
        JumpDurationStartTime = JumpDuration;
        AnimationValuePairs = new Dictionary<ObjectStates, AnimationClip>()
        {
            {ObjectStates.Idle, IdleAnim },
            {ObjectStates.Walking, WalkAnim }
        };
        ChangeGolemType();
    }

    private void Update()
    {
        UpdateAnimationState();
        CheckAnimationState();
    }

    public void ChangeGolemType()
    {
        int formerGolemType = currentGolemType;
        do
        {
            currentGolemType = Random.Range(0, 3);
        } while (formerGolemType == currentGolemType);    

        MoveSpeedMultiplier = GolemDataObjects[currentGolemType].MoveSpeedMultiplier;
        MaxMoveSpeed = GolemDataObjects[currentGolemType].MaxMoveSpeed;
        JumpDuration = GolemDataObjects[currentGolemType].JumpDuration;
        JumpSpeed = GolemDataObjects[currentGolemType].JumpSpeed;

        ActiveState = ObjectStates.Idle;
        CheckAnimationState();

        for (int i = 0; i < CharacterMeshObjects.Count; i++)
        {
            if (i == currentGolemType)
            {
                CharacterMeshObjects[i].SetActive(true);
            }
            else
            {
                CharacterMeshObjects[i].SetActive(false);
            }
        }

        RotateGolem(Rigidbody.velocity.x);
        Animator = GetComponentInChildren<Animator>();

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
            RotateGolem(input.x);
        }
        else
        {
            IsMoving = false;
        }
    }

    private void RotateGolem(float xInput)
    {
        if (xInput >= 0)
        {
            CharacterMeshObjects[currentGolemType].transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            CharacterMeshObjects[currentGolemType].transform.rotation = Quaternion.Euler(0, 270, 0);
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
