using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class DrawSword : PlayerMotion
{
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

	public override void Initialize()
	{
		base.Initialize();
		InsertOrder = InterruptedOrder = 5;
		AniName = "switch_fight_R";
		beginTs = new StateType[]
		{
			StateType.IsFighting,
			StateType.IsSwitching,
		};

		endFs = new StateType[]
		{
			StateType.IsSwitching,
		};

		checkFs = new StateType[]
		{
			StateType.IsDied,
			StateType.IsMoving,
			StateType.IsFighting,
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
		if (base.Interrupt(isorder, itorder))
		{
			pa.SwitchWeapon();
			return true;
		}
		return false; ;
	}
}
