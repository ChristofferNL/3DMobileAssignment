using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    public UnityEvent<float> EventMoveInput;
    public UnityEvent<bool> EventJumpInput;
    public UnityEvent<bool> EventAttackInput;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AttackInputEvent(bool isAttacking)
    {
        EventAttackInput.Invoke(isAttacking);
    }

    public void MoveInputEvent(float input)
    {
        EventMoveInput.Invoke(input);
    }

    public void JumpInputEvent(bool isJumping)
    {
        EventJumpInput.Invoke(isJumping);
    }
}
