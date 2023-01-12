using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    private CharacterBlackGolem blackGolem;
    private CharacterGreenGolem greenGolem;
    private CharacterGreyGolem greyGolem;

    public List<Character> characters = new();

    public enum GolemState { Black, Green, Grey };
    public GolemState ActiveGolem;

    private void Awake()
    {
        UIManager.Instance.ChangeGolemButton.onClick.AddListener(ChangeGolem);
    }
    private void Start()
    {
        blackGolem = GetComponent<CharacterBlackGolem>();
        greenGolem = GetComponent<CharacterGreenGolem>();
        greyGolem = GetComponent<CharacterGreyGolem>();
        characters.Add(blackGolem);
        characters.Add(greenGolem);
        characters.Add(greyGolem);
        ChangeGolem();
    }

    public void InitializeGolem(int index)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (i == index)
            {
                characters[i].InitializeGolem();
            }
            else
            {
                characters[i].UnInitializeGolem();
            }
        }
    }

    public void ChangeGolem()
    {
        int startingGolem = (int)ActiveGolem;
        do
        {
            ActiveGolem = (GolemState)Random.Range(0, 3);
        } while (startingGolem == (int)ActiveGolem);
        InitializeGolem((int)ActiveGolem);
    }
}