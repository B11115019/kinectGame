using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillVerticalCast : SkillCastBase
{
	public Collider TargetC;
	public Collider SelfC;

	public float IniSpeed = 500f;
	public float CamFollowSpeed = 50f;

	RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

	public override void Initialize()
	{
		base.Initialize();
		TargetC ??= GameObject.FindWithTag("Monster").GetComponent<Collider>();
		SelfC ??= transform.parent.GetComponent<Collider>();
	}

	public override void Begin(int direction = 1)
	{
		base.Begin(direction);
		Physics.IgnoreCollision(SelfC, TargetC);
		pt.rd.velocity = forward * IniSpeed;
		pt.CamFollow = false;
		pos = pt.transform.position;
	}

	public override void InMotion()
	{
		
		if (Physics.Raycast(transform.position, transform.forward, out hit, 0.1f, LayerMask.GetMask("Scene")))
		{
			pt.StopMove();
			return;
		}
		base.InMotion();
	}

	public override void End()
	{
		base.End();
		Physics.IgnoreCollision(SelfC, TargetC, false);
		pt.StopMove();
		pt.CamFollow = true;
		pt.CamFollowSpeed = CamFollowSpeed;
	}

	public override void AniEvent(string info)
	{
		base.AniEvent(info);
		CancelInvoke("Inmotion");
		pt.StopMove();
		pt.CamFollow = true;
		pt.CamFollowSpeed = CamFollowSpeed;
		Attack a = atk;
		a.Ratio += ab.Abilities[Enums.AbilityType.SkillCompletion].First * 0.1f;
		foreach (var c in Physics.OverlapCapsule(pos, pt.transform.position, Radius, LayerMask.GetMask("monster")))
		{
			print("attack " + c.name);
			cb.AttackEvent(a, c.GetComponent<CombatBase>());
		}

	}
}
