using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Block : PlayerMotion
{
	Buff bf;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

	public override void Initialize()
	{
		base.Initialize();
		InsertOrder = 2;
		InterruptedOrder = 1000;
		ArmorLevel = 1;
		AniTime = -1;
		beginTs = new StateType[]
		{
			StateType.IsBlocking
		};
		checkTs = new StateType[]
		{
			StateType.IsFighting,
		};
		checkFs = new StateType[]
		{
			StateType.IsDied,
			StateType.IsMoving,
			StateType.IsControlled,
			StateType.IsBlocking
		};
		bf = new Buff(AbilityType.Df, 0.4f);
	}

	public override void Begin(int direction = 1)
	{
		base.Begin(direction);
		pa.SetTrigger(AniTrigger.Block);
		ab.AddBuff(bf);
	}

	public override bool Interrupt(int isorder, int itorder)
	{
		if (base.Interrupt(isorder, itorder))
		{
			st.RecoverArmorLevel();
			ab.RemoveBuff(bf);
			return true;
		}	
		return false;
	}
}
