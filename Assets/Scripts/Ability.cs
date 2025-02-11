using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using System;
using Enums;

namespace Enums
{
    public enum AbilityType
	{
        Hp,
        Sp,
        Mp,

        Atk,
        Def,
        Af, // attack freq
        Ct, // critical rate
        Rct, // resist critical
        Df, // damage free
        Ed, // enhance damage

        SkillCompletion
    }
}

public struct Buff
{
    public AbilityType At;
    public Ability Target;
    public float Val;
    public float Time;
    

    public Buff(AbilityType at, float val, float time = -1)
    { 
        this.At = at;
        this.Val = val;
        this.Time = time;
        Target = null;
	}

    public IEnumerator BuffTimer()
    {
        yield return new WaitForSeconds(Time);
        Target?.RemoveBuff(this);
    }
}

public class Ability : MonoBehaviour
{
    public delegate void MinEventHandler(AbilityType at);
    public delegate void MaxEventHandler(AbilityType at);
    public delegate void IncreaseEventHandler(AbilityType at, float cur);
    public delegate void DecreaseEventHandler(AbilityType at, float cur);
    public delegate void DiedEventHandler(bool isWin);

    public MinEventHandler MinEvent;
    public MaxEventHandler MaxEvent;
    public IncreaseEventHandler IncreaseEvent;
    public DecreaseEventHandler DecreaseEvent;
    public static DiedEventHandler DiedEvent;

    public Dictionary<AbilityType, Pair<float, Tuple<float, float>>> Abilities = new Dictionary<AbilityType, Pair<float, Tuple<float, float>>>();
    
    [SerializedDictionary]
    public SerializedDictionary<AbilityType, float> CurMap, MinMap, MaxMap;

    Dictionary<AbilityType, float> BuffAbilities = new Dictionary<AbilityType, float>();

    public List<Buff> Buffs = new List<Buff>();

    public GameObject HealthUI;

    HealthUIBase ui;

    public float SpRecoverTime = 5f;
    bool spRecovering = false;

    public PlayerController pc;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

	private void Awake()
	{
        MinEvent = null;
        MaxEvent = null;
        IncreaseEvent = null;
        DecreaseEvent = null;
        DiedEvent = null;
    }

	public void Initialize()
	{
        foreach(KeyValuePair<AbilityType, float> pair in CurMap)
		{
            Abilities[pair.Key] = new Pair<float, Tuple<float, float>>(pair.Value, new Tuple<float, float>(
                MinMap.ContainsKey(pair.Key) ? MinMap[pair.Key] : 0f, MaxMap.ContainsKey(pair.Key) ? MaxMap[pair.Key] : float.MaxValue));
		}

        ui = HealthUI.GetComponent<HealthUIBase>();
        IncreaseEvent += ui.HealthChange;
        DecreaseEvent += ui.HealthChange;
        DiedEvent += pc.End;
        Invoke("InvokeInitialize", 0);
	}

    public void InvokeInitialize()
	{
        if(Abilities.ContainsKey(AbilityType.Hp))
            ui.IniHealth(AbilityType.Hp, Abilities[AbilityType.Hp].First, Abilities[AbilityType.Hp].Second.Item2 );
        if(Abilities.ContainsKey(AbilityType.Sp))
            ui.IniHealth(AbilityType.Sp, Abilities[AbilityType.Sp].First, Abilities[AbilityType.Sp].Second.Item2 );
        if(Abilities.ContainsKey(AbilityType.Mp))
            ui.IniHealth(AbilityType.Mp, Abilities[AbilityType.Mp].First, Abilities[AbilityType.Mp].Second.Item2 );
        if (Abilities.ContainsKey(AbilityType.SkillCompletion))
            ui.IniHealth(AbilityType.SkillCompletion, Abilities[AbilityType.SkillCompletion].First, Abilities[AbilityType.SkillCompletion].Second.Item2);
        ui.HealthUpdateAll();
    }

	public void Increase(AbilityType at, float num)
	{
        if (num == -1) Abilities[at].First = Abilities[at].Second.Item2;
        else if (num <= 0) return;
        else 
            Abilities[at].First = Mathf.Clamp(Abilities[at].First + num, Abilities[at].Second.Item1, Abilities[at].Second.Item2);
        Effect(at);
        if (IncreaseEvent != null) IncreaseEvent(at, Abilities[at].First);
        if (Abilities[at].First == Abilities[at].Second.Item2 && MaxEvent != null)
            MaxEvent(at);
	}

    public void Decrease(AbilityType at, float num)
    {
        if (num == -1) Abilities[at].First = Abilities[at].Second.Item1;
        else if (num <= 0) return;
        else
            Abilities[at].First = Mathf.Clamp(Abilities[at].First - num, Abilities[at].Second.Item1, Abilities[at].Second.Item2);
        Effect(at);
        if (DecreaseEvent != null) DecreaseEvent(at, Abilities[at].First);
        if (Abilities[at].First == Abilities[at].Second.Item1)
		{
            if(MinEvent != null) MinEvent(at);
            if(at == AbilityType.Hp && DiedEvent != null)
			{
                DiedEvent(tag == "Monster");
			}
		}
            
    }

    void Effect(AbilityType at)
    {
		switch (at)
		{
            case AbilityType.Sp:
				if (!spRecovering)
				{
                    spRecovering = true;
                    Invoke("RecoverSp", SpRecoverTime);
				}
                break;
		}
    }

    public void AddBuff(Buff bf)
	{
        bf.Target = this;
        Buffs.Add(bf);
        if (bf.Val > 0)
            Increase(bf.At, bf.Val);
        else
            Decrease(bf.At, -bf.Val);
        if (bf.Time <= 0) return;
        StartCoroutine(bf.BuffTimer());
	}

    public void RemoveBuff(Buff bf)
	{
        Buffs.Remove(bf);
        if (bf.Val > 0)
            Decrease(bf.At, bf.Val);
        else
            Increase(bf.At, -bf.Val);
    }

    void RecoverSp()
	{
        Increase(AbilityType.Sp, 1);
        if(Abilities[AbilityType.Sp].First < Abilities[AbilityType.Sp].Second.Item2)
            Invoke("RecoverSp", SpRecoverTime);
		else
            spRecovering = false;
	}

    public bool CheckEnough(AbilityType at, float num)
	{
        return Abilities[at].First - num >= Abilities[at].Second.Item1;
	}
}
