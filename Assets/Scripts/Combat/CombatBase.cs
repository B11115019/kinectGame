using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Enums;

namespace Enums
{
    public enum ControlType
    {
        repulse,
        stiff,
    }
}

public struct Attack
{
    public ControlType? ct;
    public int ControlLevel;
    public float ControlTime;
    public float Ratio;
    public float force;
    public Dictionary<AbilityType, Pair<float, Tuple<float, float>>> ab;
}

public class CombatBase: MonoBehaviour
{

    public delegate void AttackEventHandler(Attack atk, CombatBase target);
    public delegate bool InjuredEventHandler(Attack atk);
    public delegate void InjuredMotionEffectHandler(Attack atk);
    public delegate void ControlledEventHandler(Attack atk);

    public AttackEventHandler AttackEvent;
    public InjuredEventHandler InjuredEvent;
    public InjuredMotionEffectHandler InjuredEffectEvent;
    public ControlledEventHandler ControlledEvent;

    public Ability ability;

    public StateBase sb;

	private void Start()
	{
        Initialize();
	}

	private void Awake()
	{
        AttackEvent = null;
        InjuredEvent = null;
        InjuredEffectEvent = null;
        ControlledEvent = null;
    }
	virtual public void Initialize()
	{
        ability ??= GetComponent<Ability>();
        sb ??= GetComponent<StateBase>();
        InjuredEvent += Injured;
        AttackEvent += Attack;
	}

	virtual public void Attack(Attack atk, CombatBase target)
    {
        if(target?.InjuredEvent != null)
        {
            target?.InjuredEvent(atk);
        }
            
    }

    virtual public bool Injured(Attack a)
	{
        if (!CanInjured() || a.ab == null) return false;
        int damage = (int)(Mathf.Pow(a.ab[AbilityType.Atk].First, 2) / ability.Abilities[AbilityType.Def].First *
            (1 + a.ab[AbilityType.Ed].First - ability.Abilities[AbilityType.Df].First) * a.Ratio);

        if (UnityEngine.Random.Range(0f, 1f) < a.ab[AbilityType.Ct].First - ability.Abilities[AbilityType.Rct].First)
            damage = (int)(damage * 1.5);

        ability.Decrease(AbilityType.Hp, damage);

        GetControl(a);

        if(InjuredEffectEvent != null)
            InjuredEffectEvent(a);

        return true;
    }

    virtual public bool CanInjured()
	{
        return !sb.GetState(StateType.IsInvincible);
	}

    virtual public bool CheckControl(Attack a)
	{
        return a.ct != null && a.ControlLevel > sb.ArmorLevel;
	}

    virtual public void GetControl(Attack a)
	{
        if (!CheckControl(a)) return;
        sb.SetControlled(a.ControlTime);
        if(ControlledEvent != null)
            ControlledEvent(a);
    }
}
