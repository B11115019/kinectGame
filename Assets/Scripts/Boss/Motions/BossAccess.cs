using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class BossAccess : BossMotion
{
	public float SpeedUp = 2;
	void Start()
	{
		Initialize();
	}

	public override void Initialize()
	{
		base.Initialize();
		InsertOrder = InterruptedOrder = 49;
		AniSpeed = 4;
		checkFs = new StateType[]
		{
			StateType.IsDied,
			StateType.IsControlled
		};
	}

	public override void Begin(int direction = 1)
	{
		CancelInvoke("InMotion");
		CancelInvoke("End");
		Invoke("InMotion", 0);
		st.SetState(beginTs, beginFs);
		animator.SetTrigger("accessTrigger");
	}

	public override void InMotion()
	{
		
		if (bc.AccessTarget(SpeedUp))
			base.InMotion();
		else
		{
			End();
		}
	}
	public override void End()
	{
		base.End();
		bc.AccessEnd();
	}
}
