using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enums
{
    public enum InputType
    {
        Begin = KeyCode.Return,
        Walk = -2,
        Run = -1,
        SwitchPeace = KeyCode.U,
        SwitchFight = KeyCode.I,
        Block = KeyCode.B,
        AttackLD = KeyCode.H, // attack from left down to right up
        AttackLU = KeyCode.J,
        AttackRD = KeyCode.K,
        AttackRU = KeyCode.L,
        TriggerUI = KeyCode.P,
        SkillVerticeReady = KeyCode.Alpha1,
        SkillVerticeCast = KeyCode.Alpha2,
    }
}
