using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;


public class MotionBase : MonoBehaviour
{
    public AnimationClip ani;
    public StateBase st;
    public CombatBase cb;
    public Ability ab;
    public string AniName;
    public float AniTime;
    public float AniSpeed = 1f;
    public int InsertOrder = int.MaxValue, InterruptedOrder = int.MaxValue;// smallest first
    public StateType[] beginTs, beginFs, endTs, endFs, checkTs, checkFs;
    public float RecoverTime = 0;
    public bool IsRecovering = false;
    public short ArmorLevel = 0;

    public delegate void DiedEventHandler();
    public Dictionary<AbilityType, float> Cost;

    MotionManager mm;

    void Start()
    {
        Initialize();
    }

    public virtual void Initialize() 
    {
        st ??= transform.parent.GetComponent<StateBase>();
        cb ??= transform.parent.GetComponent<CombatBase>();
        ab ??= transform.parent.GetComponent<Ability>();
        if (ani == null) return;
        AniName = ani.name;
        AniTime = ani.length / AniSpeed;
    }
    public virtual bool Check(MotionBase cur) 
    {
        if (!IsRecovering && (cur != null ? (InsertOrder < cur.InterruptedOrder) : true) && st.SwitchMotionCheck(cur, this))
        {
            if (Cost == null) return true;
            foreach(KeyValuePair<AbilityType, float> p in Cost)
			{
                if (!ab.CheckEnough(p.Key, p.Value)) return false;
			}
            return true;
        }
        return false;
    }
    virtual public void Begin(int direction = 1)
    {
        CancelInvoke("InMotion");
        CancelInvoke("End");
        Invoke("InMotion", 0);
        if(AniTime >= 0) Invoke("End", AniTime);
        st.SetState(beginTs, beginFs);
        st.ArmorLevel = ArmorLevel;
        if (Cost == null) return;
        foreach (KeyValuePair<AbilityType, float> p in Cost)
        {
            ab.Decrease(p.Key, p.Value);
        }
    }

    virtual public void End()
    {
        CancelInvoke("InMotion");
        mm.MotionEnd(AniName);
        st.SetState(endTs, endFs);
        st.RecoverArmorLevel();
        SetRecover();
    }

    virtual public bool Do(int direction = 1, MotionBase cur = null) {
        if (!Check(cur) || !(cur?.Interrupt(InsertOrder, InterruptedOrder) ?? true)) return false;
        Begin(direction);
        return true;
    }

    virtual public void InMotion()
	{
        Invoke("InMotion", 0);
	}

    virtual public bool Interrupt(int isorder, int itorder)
	{
        if (isorder != -1 && InterruptedOrder <= isorder) return false;
        CancelInvoke("InMotion");
        CancelInvoke("End");
        st.RecoverArmorLevel();
        RecoverState();
        SetRecover();
        return true;
	}
    
    virtual public void AniEvent(string info)
	{
	}

    virtual public void RecoverState(bool sync = true)
	{
        st.SetState(beginFs, beginTs, sync);
	}

    public void SetMotionManager(MotionManager m)
	{
        mm = m;
	}

    public void SetRecover()
	{
        if (RecoverTime <= 0) return;
        IsRecovering = true;
        Invoke("RecoverEnd", RecoverTime);
	}

    public void RecoverEnd()
	{
        IsRecovering = false;
	}
}
