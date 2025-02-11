using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class BossAvoid : BossMotion
{

	private void Start()
	{
		Initialize();
	}
	public override void Initialize()
    {
        base.Initialize();
        InsertOrder = 5;
        InterruptedOrder = 200;
		checkFs = new StateType[]
		{
			StateType.IsDied,
			StateType.IsControlled,
		};
    }

	public override void Begin(int direction = 1)
	{
		base.Begin(direction);
        animator.SetTrigger("avoidTrigger");
		bc.LookTarget(true);
		bc.AvoidContinuously();
	}
	public override void End()
	{
		base.End();
		bc.CancelInvoke("AvoidContinuously");
		bc.rg.velocity = Vector3.zero;
	}

	public override bool Interrupt(int isorder, int itorder)
	{
		if(base.Interrupt(isorder, itorder))
		{
			bc.CancelInvoke("AvoidContinuously");
			bc.rg.velocity = Vector3.zero;
			return true;
		}
		return false;
	}
}
