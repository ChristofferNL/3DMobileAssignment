using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [HideInInspector] public Rigidbody Rigidbody;
    public float MaxMoveSpeed;
    public float JumpDuration;
    public float JumpSpeed;
    public bool canJump;
    protected float JumpDurationStartTime;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        EventManager.Instance.EventMoveInput.AddListener(Move);
        EventManager.Instance.EventJumpInput.AddListener(Jump);
        JumpDurationStartTime = JumpDuration;
    }

    public virtual void Move(Vector2 input)
    {
        if (Rigidbody.velocity.x > MaxMoveSpeed && input.x > 0 || Rigidbody.velocity.x < -MaxMoveSpeed && input.x < 0)
        {
            return;
        }
        Rigidbody.AddForce(new Vector3(input.x, 0, 0));
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
