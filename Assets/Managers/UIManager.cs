using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Button ChangeGolemButton;
    public Button ChangeEquipmentButton;
    public Button JumpButton;
    public Button SmashButton;
    public Button ExitGameButton;
    public TextMeshProUGUI DestroyedBuildingsText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        JumpButton.onClick.AddListener(ExecuteJump);
        ExitGameButton.onClick.AddListener(ExitApplication);
    }
    
    public void SetDestroyedBuildingsText(int destroyedBuildings)
    {
        DestroyedBuildingsText.text = $"Buildings Destroyed: {destroyedBuildings.ToString()}";
    }

    public void ExecuteJump()
    {
        EventManager.Instance.JumpInputEvent(true);
    }

    public void ExitApplication()
    {
        SaveManager.Instance.SaveData();
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
