using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    public UnityEvent<Vector2> EventMoveInput;
    public UnityEvent<bool> EventJumpInput;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void MoveInputEvent(Vector2 input)
    {
        EventMoveInput.Invoke(input);
    }

    public void JumpInputEvent(bool isJumping)
    {
        EventJumpInput.Invoke(isJumping);
    }
}
