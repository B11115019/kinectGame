using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class BossMove :BossMotion
{
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

	public override void Initialize()
	{
		base.Initialize();
		InsertOrder = InterruptedOrder = 50;
		AniSpeed = 2;
		AniTime = -1;
		checkFs = new StateType[]
		{
			StateType.IsControlled
		};
	}

	public override void Begin(int direction = 1)
	{
		base.Begin(direction);
		animator.SetTrigger("moveTrigger");
	}

	public override void InMotion()
	{

		if (bc.IsInChaseRange())
		{
			base.InMotion();
			bc.Chase();
		}
		else
			End();
	}
	public override void End()
	{
		base.End();
		bc.ChaseEnd();
	}
}
