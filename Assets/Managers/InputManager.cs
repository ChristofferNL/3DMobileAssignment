using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerControls inputActions;
    private PlayerControls.PlayerActionMapActions playerActions;

    private void Awake()
    {
        inputActions = new();
        playerActions = inputActions.PlayerActionMap;
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }

    private void Update()
    {

        //EventManager.Instance.MoveInputEvent(playerActions.MovementActions.ReadValue<float>());
        EventManager.Instance.JumpInputEvent(playerActions.Jump.IsPressed());
    }
}
