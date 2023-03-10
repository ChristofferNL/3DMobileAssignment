using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public int BuildingsDestroyed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        LoadData();
        UIManager.Instance.SetDestroyedBuildingsText(BuildingsDestroyed);
    }

    public void RecordBuildingDestroyed()
    {
        BuildingsDestroyed++;
        UIManager.Instance.SetDestroyedBuildingsText(BuildingsDestroyed);
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("DestroyedBuildings", BuildingsDestroyed);
    }

    public void LoadData()
    {
        BuildingsDestroyed = PlayerPrefs.GetInt("DestroyedBuildings");
    }
}
