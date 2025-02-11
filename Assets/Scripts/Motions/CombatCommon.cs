using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class CombatCommon : PlayerMotion
{
	public float ForwardOffset = 2f, YOffset = 2f;

	public float Radius = 2f;

	public float Ratio = 0.5f;

	public float Force = 0;
		
	public int AttackMode = 1;

	public int ControlLevel = 1;
	public float ControlTime = 0.1f;

	Vector3 pos;

	Attack atk;

	public override void Initialize()
	{
		base.Initialize();
		InsertOrder = InterruptedOrder = 49;

		beginTs = endFs = new StateType[]
		{
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

	public override void Begin(int direction = 1)
	{
		base.Begin(direction);
		pa.SetInt(AniInt.AttackMode, AttackMode);
		pa.SetTrigger(AniTrigger.Attack);
		pt.StopMove();
	}

	public override void End()
	{
		base.End();
		st.SetRecovery();
	}

	public override void AniEvent(string info)
	{
		base.AniEvent(info);
		pos = transform.position + transform.forward * ForwardOffset + Vector3.up * YOffset;
		foreach (var c in Physics.OverlapSphere(pos, Radius, LayerMask.GetMask("monster")))
		{
			print("attack " + c.name);
			cb.AttackEvent(atk, c.GetComponent<CombatBase>());
		}
	}

	public void OnDrawGizmosSelected()
	{
		pos = transform.position + transform.forward * ForwardOffset + Vector3.up * YOffset;
		Gizmos.DrawSphere(pos, Radius);
	}
}
