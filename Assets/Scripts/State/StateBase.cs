using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System.Linq;

namespace Enums
{
    public enum StateType
    {
        IsDied = 0,
        IsControlled = 1,
        IsMoving = 2,
        IsBlocking = 3,
        IsSkillReading = 4,
        IsInvincible = 5,
        IsFighting = 6,
        IsSwitching = 7,
        IsAttacking = 8,
        IsRecovering = 9,//attack interval
        InMotion = 10,
        CanMove = 11,
        CanRotate = 12,
    }
}

public class StateBase : MonoBehaviour
{
    public short ArmorLevel = 0;
    public uint States = 0;
    public float AttackInterval = 0.5f;
    public short DefaultArmorLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    virtual public void Initialize()
	{
        States = 0;
        ArmorLevel = 0;
        SetState(StateType.CanMove); 
        SetState(StateType.CanRotate);
    }

    virtual public bool CheckState(StateType[] trueSt, StateType[] falseSt = null, bool allSame = false)
    {
        int i = 0;
        uint s = States;

        foreach (StateType st in trueSt ?? Enumerable.Empty<StateType>())
        {
            i |= 1 << (int)st;
        }

        foreach (StateType st in falseSt ?? Enumerable.Empty<StateType>())
        {
            i |= 1 << (int)st;
            s ^= (uint)1 << (int)st;
        }
        if (!allSame)
            return (s & i) != 0;
        return (s & i) == i;
    }

    virtual public bool GetState(StateType st)
    {
        return (States & (uint)1 << (int)st) != 0;
    }

    virtual public void SetState(StateType st, bool isTrue = true)
    {
        if (((States & ((uint)1 << (int)st)) != 0) != isTrue)
            States ^= (uint)1 << (int)st;
        SyncAniBool();
    }

    virtual public void SetState(StateType[] trueSt = null, StateType[] falseSt = null, bool sync = true)
    {
        foreach (StateType st in trueSt ?? Enumerable.Empty<StateType>())
        {
            States |= (uint)1 << (int)st;
        }
        foreach (StateType st in falseSt ?? Enumerable.Empty<StateType>())
        {
            if ((States & (uint)1 << (int)st) != 0)
                States ^= (uint)1 << (int)st;
        }
        if (sync) SyncAniBool();
    }

    virtual public void SetDied()
    {
        SetState(StateType.IsDied);
        StopAllCoroutines();
        SyncAniBool();
    }

    virtual public void SetControlled(float time, ControlType ct = ControlType.stiff)
    {
        SetState(StateType.IsControlled);
        SetState(StateType.CanRotate, false);
        CancelInvoke("ControlEnd");
        Invoke("ControlEnd", time);
    }

    virtual public void ControlEnd()
    {
        SetState(StateType.IsControlled, false);
        SetState(StateType.CanRotate);
        SyncAniBool();
    }

    virtual public void SyncAniBool()
    {
    }

    virtual public bool SwitchMotionCheck(MotionBase prev, MotionBase cur)
    {
        uint temp = States;
        if (prev != null) prev.RecoverState(false);
        bool result = !CheckState(cur.checkFs, cur.checkTs);
        States = temp;
        return result;
    }

    virtual public void SetRecovery(float? f = null)
    {
        SetState(StateType.IsRecovering);
        Invoke("RecoveryEnd", f ?? AttackInterval);
    }

    virtual public void RecoveryEnd()
    {
        SetState(StateType.IsRecovering, false);
    }

    public void RecoverArmorLevel()
	{
        ArmorLevel = DefaultArmorLevel;
	}
}
