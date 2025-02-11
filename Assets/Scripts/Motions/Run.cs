using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Run : PlayerMotion
{
    public float runStep = 20;

	public float InjuredTimeScale = 0.5f;
	public float InjuredEffectLen = 0.3f;
	float oriTimeScale; 

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

	public override void Initialize()
	{
		base.Initialize();
        InsertOrder = InterruptedOrder = 1;
		endFs = beginTs = new StateType[]
		{
			StateType.IsMoving,
			StateType.IsInvincible
		};
		checkTs = new StateType[]
		{
			StateType.IsFighting,
			StateType.CanMove
		};
		checkFs = new StateType[]
		{
			StateType.IsDied,
			StateType.IsMoving,
			StateType.IsControlled
		};
		Cost = new Dictionary<AbilityType, float>
		{
			{ AbilityType.Sp, 1 },
		};
	}

	public override void Begin(int direction = 1)
	{
		base.Begin(direction);
        pt.SetModelForward(direction);
        pt.MoveByModelForward(runStep);
		pa.SetTrigger(AniTrigger.MoveInFight);
		cb.InjuredEvent += InjuredEffect;
    }

	public override void InMotion()
	{
		base.InMotion();
        pt.MoveByModelForward(runStep);
    }

	public override void End()
	{
		base.End();
        pt.ResetModelForward();
		pt.StopMove();
		cb.InjuredEvent -= InjuredEffect;
	}

	public override bool Interrupt(int isorder, int itorder)
	{
		if(base.Interrupt(isorder, itorder))
		{
			pt.ResetModelForward();
			pt.StopMove();
			return true;
		}
		return false;
	}

	public bool InjuredEffect(Attack at)
	{
		oriTimeScale = Time.timeScale;
		Time.timeScale = InjuredTimeScale;
		Invoke("StopEffect", InjuredEffectLen);
		return false;
	}

	public void StopEffect()
	{
		Time.timeScale = oriTimeScale;
	} 
}
