using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PutSword : PlayerMotion
{
	// Start is called before the first frame update
	void Start()
	{
		Initialize();
	}

	public override void Initialize()
	{
		base.Initialize();
		InsertOrder = InterruptedOrder = 99;
		AniName = "switch_peace";
		beginTs = new StateType[]
		{
			StateType.IsSwitching
		};

		beginFs = new StateType[]
		{
			StateType.IsFighting,
		};

		endFs = new StateType[]
		{
			StateType.IsSwitching
		};

		checkTs = new StateType[]
		{
			StateType.IsFighting
		};

		checkFs = new StateType[]
		{
			StateType.IsDied,
			StateType.IsMoving,
			StateType.IsControlled,
			StateType.IsSwitching
		};
	}
	public override void Begin(int direction = 1)
	{
		base.Begin(direction);
		pa.SetTrigger(AniTrigger.Switch);
	}

	public override void AniEvent(string info)
	{
		base.AniEvent(info);
		pa.SwitchWeapon();
	}

	public override bool Interrupt(int isorder, int itorder)
	{
		if(base.Interrupt(isorder, itorder)){
			pa.SwitchWeapon();
			return true;
		}
		return false; ;
	}
}
