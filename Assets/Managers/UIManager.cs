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
    public List<CharacterDataSO> GolemDataObjects = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        ChangeGolemButton.onClick.AddListener(ChangeGolem);
    }
    public void ChangeGolem()
    {
        CharacterDataSO dataSO = GolemDataObjects[Random.Range(0, GolemDataObjects.Count)];
        EventChangeGolem.Invoke(dataSO);
    }
}
