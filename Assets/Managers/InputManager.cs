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
        Application.targetFrameRate = 60;
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
        //EventManager.Instance.AttackInputEvent(playerActions.Attack.IsPressed());
        //EventManager.Instance.MoveInputEvent(playerActions.MovementActions.ReadValue<float>());
        //EventManager.Instance.JumpInputEvent(playerActions.Jump.IsPressed());
    }
}
