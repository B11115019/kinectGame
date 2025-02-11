using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class SkillCastBase : PlayerMotion
{
	public float ForwardOffset = 0.8f, YOffset = 0.6f;

	public float Radius = 0.5f;

	public float Ratio = 0.5f;

	public float Force = 0;

	public int AttackMode = 1;

	public int ControlLevel = 1;
	public float ControlTime = 0.1f;
	

	public Vector3 pos;
	public Vector3 forward;

	public Attack atk;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

	public override void Initialize()
	{
		base.Initialize();
		InsertOrder = 47;
		InterruptedOrder = -1;

		beginTs = endFs = new StateType[]
		{
			StateType.IsInvincible,
			StateType.IsAttacking,
		};

		beginFs = endTs = new StateType[]
		{
			StateType.CanRotate,
		};

		checkTs = new StateType[]
		{
			StateType.IsFighting,
		};

		checkFs = new StateType[]
		{
			StateType.IsDied,
			StateType.IsControlled,
			StateType.IsSwitching,
			StateType.IsRecovering,
			StateType.IsAttacking
		};

		atk = new Attack
		{
			ct = ControlType.stiff,
			ControlLevel = ControlLevel,
			ControlTime = ControlLevel,
			Ratio = Ratio,
			force = Force,
			ab = this.ab.Abilities
		};
	}

	public override bool Check(MotionBase cur)
	{
		return base.Check(cur) && pa.ani.GetInteger("AttackMode") == AttackMode && st.GetState(StateType.IsSkillReading);
	}

	public override void Begin(int direction = 1)
	{
		base.Begin(direction);
		pa.SetTrigger(AniTrigger.SkillCast);
		pt.StopMove();
		forward = pt.transform.forward;
	}
}
