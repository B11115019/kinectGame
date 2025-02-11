using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

public class BossMotion : MotionBase
{
    public Animator animator;
    public BossController bc;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

	public override void Initialize()
	{
		base.Initialize();
        animator ??= GetComponent<Animator>();
        bc ??= transform.parent.GetComponent<BossController>();
    }

	public override void End()
	{
		base.End();
        bc.MotionEnd();
	}
}