using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class BossCombatCommon : BossMotion
{
    public float ForwardOffset = 1f, YOffset = 1f;

    public float Radius = 0.5f;

    public float Ratio = 0.5f;
    public float Force = 30;
   
    public int AttackMode = 1;

    public int ControlLevel = 1;
    public float ControlTime = 0.5f;
    
    protected Attack atk;
    protected Vector3 forward = Vector3.zero;

    protected Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
	{   
        Initialize();
        InsertOrder = InterruptedOrder = 101;

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
        base.Begin(direction);
        animator.SetInteger("AttackMode", AttackMode);
        animator.SetTrigger("attackTrigger");
        bc.LookTarget(true);
        forward =bc.transform.forward;
        forward.y = 0;
    }

    public override void AniEvent(string info)
    {
        base.AniEvent(info);
        pos = transform.position + transform.forward * ForwardOffset + Vector3.up * YOffset;
        foreach (var c in Physics.OverlapSphere(pos, Radius, LayerMask.GetMask("player")))
        {
            print("attack " + c.name);
            cb.Attack(atk, c.GetComponent<CombatBase>());
        }
    }

	public override void End()
	{
		base.End();
        bc.LookTarget(true);
    }

	public void OnDrawGizmosSelected()
    {
        pos = transform.position + transform.forward * ForwardOffset + Vector3.up * YOffset;
        Gizmos.DrawSphere(pos, Radius);
    }
}
