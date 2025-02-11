using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PlayerMotion : MotionBase
{
    public PlayerAni pa;
    public PlayerEventTrigger pet;
    public PlayerTransform pt;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    
    public override void Initialize()
    {
        base.Initialize();
        pa ??= GetComponent<PlayerAni>();
        pet ??= transform.parent.GetComponent<PlayerEventTrigger>();
        pt ??= transform.parent.GetComponent<PlayerTransform>();
    }
}
