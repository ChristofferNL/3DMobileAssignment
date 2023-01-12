using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveLeftButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool buttonPressed;

    private void Update()
    {
        if (buttonPressed)
        {
            EventManager.Instance.MoveInputEvent(-1.00f);
        }
        else
        {
            EventManager.Instance.MoveInputEvent(0f);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
}
