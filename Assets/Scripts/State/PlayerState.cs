using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PlayerState : StateBase
{
    PlayerAni pa;

    Dictionary<StateType, string> aniBoolMap;

    // Start is called before the first frame update
    void Awake()
    {
        pa ??= GetComponentInChildren<PlayerAni>();
        aniBoolMap = new Dictionary<StateType, string>
        {
            { StateType.IsDied, "isDied" },
            { StateType.IsControlled, "isControlled" },
            { StateType.IsMoving, "isMoving" },
            { StateType.IsBlocking, "isBlocking" },
            { StateType.IsFighting, "isFighting" },
        };
    }

	public override void SyncAniBool()
	{
        foreach(KeyValuePair<StateType, string> p in aniBoolMap)
		{
            pa.SetBool(p.Value, GetState(p.Key));
		}
	}

	public override void SetControlled(float time, ControlType ct = ControlType.stiff)
	{
		base.SetControlled(time, ct);
        pa.SetTrigger(AniTrigger.Contrlled);
	}
}
