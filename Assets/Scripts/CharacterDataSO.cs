using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GolemTypeData")]
public class CharacterDataSO : ScriptableObject
{
    public float MoveSpeedMultiplier;
    public float MaxMoveSpeed;
    public float JumpDuration;
    public float JumpSpeed;

    public AnimationClip IdleAnim;
    public AnimationClip WalkAnim;
}
