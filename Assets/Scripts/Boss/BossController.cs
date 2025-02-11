using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enums;

public class BossController : MonoBehaviour
{
    public NavMeshAgent na;
    public Transform target;
    public Animator ani;
    public StateBase st;
    public BossSM bsm;
    public MotionManager mm;
    public Rigidbody rg;

    public float AttackRange = 1f;
    public float AccessRange = 3;
    //public float ChaseRange = 2.5f;
    public float MissRange = 0.3f;
    public float RotateSpeed = 90f;
    public float AvoidRate = 0.1f;
    public float MoveSpeed = 11;
    public bool InMotion = false;
    public bool InStay = true;
    public float AvoidSpeed = 20f;

    bool isEnding = false;
    

    

    Dictionary<BossState, List<string>> stateMotionMap = new Dictionary<BossState, List<string>>
    {
        { BossState.Chase, new List<string>{"move"} },
        { BossState.Stay,  new List<string>{"stay"} },
        { BossState.Avoid, new List<string>{"avoid"} },
        { BossState.Access, new List<string>{"access"} },        
        { BossState.Attack, new List<string>{"combat1", "combat2" , "combat3" , "combat4", "skill_rotate" } },
    };

    // Start is called before the first frame update
    void Start()
    {
        Ability.DiedEvent += Died;
    }

    private void Awake()
    {
        bsm ??= GetComponent<BossSM>();
        ani ??= GetComponentInChildren<Animator>();
        st ??= GetComponent<StateBase>();
        mm ??= GetComponentInChildren<MotionManager>();
        rg ??= GetComponent<Rigidbody>();
    }

	// Update is called once per frame
	void Update()
    {
        if (isEnding) return;
        if (st.GetState(StateType.CanRotate)) LookTarget();
        DoMotion();
    }

    public void LookTarget(bool imm = false)
	{
        Vector3 dir = target.position - transform.position;
        dir.y = 0;
        float angle = Vector3.Angle(transform.forward, Vector3.Normalize(dir));
        if (imm)
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        else
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir, Vector3.up), RotateSpeed * Time.deltaTime / angle);
    }

    void DoMotion()
	{
        if (InMotion) return;
		switch (bsm.bs)
		{
            case BossState.Stay:
                if (InStay)
                {
                    bsm.StateChange();
                    return;
                }
                else
                {
                    ani.SetTrigger("stayTrigger");
                    InStay = true;
                    return;
                }
            default:
                InStay = false;
                InMotion = true;
                string m = null;
                float p = 1f / stateMotionMap[bsm.bs].Count;
                string ms = null;
                foreach (string s in stateMotionMap[bsm.bs])
                {
                    if (mm.GetMotion(s).IsRecovering) continue;
                    if (ms == null) ms = s;
                    if (Random.Range(0f, 1f) <= p) m = s;
                }
                if (!mm.DoMotion(m ?? ms))
                    MotionEnd(true);
                return;
        }
    }

    public void MotionEnd(bool necessaryChange = false)
	{
        InMotion = false;
        bsm.StateChange(necessaryChange);
	}

    public void Chase()
    {
        na.speed = MoveSpeed;
        na.SetDestination(GetAttackPos(1.7f));
    }

    public void ChaseEnd()
	{
        na.SetDestination(transform.position);
        bsm.StateChange();
	}

    Vector3 GetAttackPos(float ratio = 1)
    {
        return target.position - Vector3.Normalize(target.position - transform.position) * AttackRange;
    }

    public bool AccessTarget(float speedUp = 4)
	{
        na.speed = MoveSpeed * speedUp;
        na.SetDestination(GetAttackPos());
        return Vector3.Distance(GetAttackPos(), transform.position) > MissRange;
    }

    public void AccessEnd()
	{
        na.speed = MoveSpeed;
        na.SetDestination(transform.position);
    }
    
    public bool IsInAttackRange()
	{
        return Vector3.Distance(transform.position, target.position) <=  AttackRange + MissRange;
    }

    public bool IsInAccessRange()
	{
        return Vector3.Distance(transform.position, target.position) <= AccessRange;
    }

    public bool IsInChaseRange()
	{
        return Vector3.Distance(transform.position, target.position) > AccessRange;
    }

    public void AvoidContinuously()
	{
        rg.velocity = -transform.forward * AvoidSpeed;
        Invoke("AvoidContinuously", 0);
	}

    public void Died(bool isWin)
	{
        mm.InterruptMotion();
        AccessEnd();
        isEnding = true;
        if (isWin)
        {
            ani.SetTrigger("diedTrigger");
            bsm.bs = BossState.Died;
        };
	}
}
