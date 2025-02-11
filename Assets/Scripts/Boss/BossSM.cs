using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

namespace Enums
{
    public enum BossState
	{
        Chase,
        Avoid,
        Attack,
        Kite,
        Stay,
        Access,
        Died,
    }
}

public class BossSM : MonoBehaviour
{

    public Ability ab;
    public BossController bc;

    public float AttackInterval = 1;

    public BossState bs = BossState.Stay;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Initialize()
	{
        bc ??= GetComponent<BossController>();
        ab ??= GetComponent<Ability>();
	}


    public void StateChange(bool necessary = false)
	{
        // in motion do
		switch (bs)
		{
            case BossState.Chase:
                if (bc.IsInAttackRange())
                    bs = BossState.Attack;
                else if (bc.IsInAccessRange())
                    bs = BossState.Access;
                else if (!bc.IsInChaseRange())
                    bs = BossState.Stay;
                break;
            case BossState.Access:
                if (bc.IsInAttackRange())
                    bs = BossState.Attack;
                return;
            case BossState.Attack:
                if (bc.IsInChaseRange())
                    bs = BossState.Chase;
                else if (necessary || Random.Range(0f, 1f) < bc.AvoidRate)
                    bs = BossState.Avoid;
                else if (!bc.IsInAttackRange())
                    bs = BossState.Access;
                break;
            case BossState.Avoid:
                bs = BossState.Chase;
                StateChange();
                break;
            case BossState.Died:
                break;
            default:
                bs = BossState.Chase;
                StateChange();
                break;
        }
    }
}
