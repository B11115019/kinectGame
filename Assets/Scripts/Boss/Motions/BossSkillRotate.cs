using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class BossSkillRotate : BossCombatCommon
{
    public Rigidbody rg;
    public float MoveSpeed = 1f;

	private void Awake()
	{
        Initialize();
        rg ??= transform.parent.GetComponent<Rigidbody>();
		AttackMode = 1;
        InsertOrder = InterruptedOrder = 101;
        ArmorLevel = 3;

        beginTs = endFs = new StateType[]
        {
            StateType.IsAttacking,
        };

        beginFs = endTs = new StateType[]
        {
            StateType.CanRotate,
        };

        checkFs = new StateType[]
        {
            StateType.IsControlled,
            StateType.IsDied,
            StateType.IsAttacking,
        };
        atk = new Attack
        {
            ct = ControlType.repulse,
            ControlLevel = ControlLevel,
            ControlTime = ControlTime,
            Ratio = Ratio,
            force = Force,
            ab = this.ab.Abilities
        };
    }

	public override void Begin(int direction = 1)
	{
        CancelInvoke("InMotion");
        CancelInvoke("End");
        Invoke("InMotion", 0);
        if (AniTime >= 0) Invoke("End", AniTime);
        st.SetState(beginTs, beginFs);
        st.ArmorLevel = ArmorLevel;
        animator.SetInteger("AttackMode", AttackMode);
        animator.SetTrigger("skillTrigger");
        bc.LookTarget(true);
        forward = Vector3.Normalize(new Vector3 ( bc.transform.forward.x, 0, bc.transform.forward.z ));
        forward *= MoveSpeed;
    }

	public override void InMotion()
	{
		base.InMotion();
        rg.velocity = forward;
	}
}
