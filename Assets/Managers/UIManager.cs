using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Button ChangeGolemButton;
    public UnityEvent<CharacterDataSO> EventChangeGolem;
    public Button ChangeEquipmentButton;
    public Button JumpButton;
    public Button MoveRightButton;
    public Button MoveLeftButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
