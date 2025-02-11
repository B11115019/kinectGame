using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Walk : PlayerMotion
{
    public float walkStep = 10;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public override void Initialize()
	{
		base.Initialize();
		InsertOrder = InterruptedOrder = 100;
		AniTime = 1;
		endFs = beginTs = new StateType[]
		{
			StateType.IsMoving,
		};
		checkTs = new StateType[]
		{
			StateType.CanMove
		};
		checkFs = new StateType[]
		{
			StateType.IsDied,
			StateType.IsMoving,
			StateType.IsControlled,
			StateType.IsFighting,
		};
	}

	public override void Begin(int direction = 1)
	{
		base.Begin(direction);
		pt.SetModelForward(direction);
		pt.MoveByModelForward(walkStep);
	}

	public override void InMotion()
	{
		base.InMotion();
		pt.MoveByModelForward(walkStep);
	}

	public override void End()
	{
		base.End();
		pt.ResetModelForward();
		pt.StopMove();
	}

	public override bool Interrupt(int isorder, int itorder)
	{
		if (base.Interrupt(isorder, itorder))
		{
			pt.ResetModelForward();
			pt.StopMove();
			return true;
		}
		return false;
	}
}
