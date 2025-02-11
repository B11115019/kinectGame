using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class SkillReadyBase : PlayerMotion
{
	public float ReadySpeed = 25f;
	bool increaseCompletion = true;

	public int AttackMode = 1;

	// Start is called before the first frame update
	void Start()
    {
        Initialize();
    }

	public override void Initialize()
	{
		base.Initialize();
		InsertOrder = 50;
		InterruptedOrder = 48;
		AniTime = -1;
		beginTs = new StateType[]
		{
			StateType.IsSkillReading,
		};
		checkTs = new StateType[]
		{
			StateType.IsFighting
		};
		checkFs = new StateType[]
		{
			StateType.IsDied,
			StateType.IsControlled,
			StateType.IsRecovering,
			StateType.IsSwitching,
			StateType.IsSkillReading,
		};
		Cost = new Dictionary<AbilityType, float> { { AbilityType.Mp, 50f} };
	}

	public override void Begin(int direction = 1)
	{
		base.Begin(direction);
		increaseCompletion = true;
		pa.SetInt(AniInt.AttackMode, AttackMode);
		pa.SetTrigger(AniTrigger.SkillReady);
		ab.Decrease(AbilityType.SkillCompletion, -1);
		ab.MinEvent += OverTime;
		ab.MaxEvent += CompletionFull;
	}

	public override void InMotion()
	{
		base.InMotion();
		if (increaseCompletion) 
			ab.Increase(AbilityType.SkillCompletion, ReadySpeed * Time.deltaTime);
		else
			ab.Decrease(AbilityType.SkillCompletion, ReadySpeed * Time.deltaTime);
	}
	
	virtual public void CompletionFull(AbilityType at)
	{
		if (at == AbilityType.SkillCompletion)
			increaseCompletion = false;
	}

	virtual public void OverTime(AbilityType at)
	{
		if(at == AbilityType.SkillCompletion)
		{
			Interrupt(-1, 0);
			pa.SetTrigger(AniTrigger.Stay);
		}
			
	}

	public override bool Interrupt(int isorder, int itorder)
	{
		if (!base.Interrupt(isorder, itorder)) return false;
		ab.MinEvent -= OverTime;
		ab.MaxEvent -= CompletionFull;
		return true;
	}
}
