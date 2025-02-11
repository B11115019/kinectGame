using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;
using System.Runtime.InteropServices;

[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerTransform))]
public class PlayerController : MonoBehaviour
{
    public PlayerState ps;
    PlayerTransform pt;
    PlayerAni pa;
    MotionManager mm;
    public CombatBase cb;
    public UIManager ui;

	public int moveDir = 0; // 0 = stop, 1 = forward, 2 = back, 3 = left, 4 = right

    public Dictionary<InputType, string> inputMap;

    public bool isEnding = false;
    public AudioManager am;


	// Start is called before the first frame update

	private void Start()
	{
        cb.InjuredEffectEvent += InjuredEffect;
        cb.ControlledEvent += ControlledEffect;
        cb.AttackEvent += HitEffect;
        InputManager.pc = this;
	}

	void Awake()
    {
        ps ??= GetComponent<PlayerState>();
        pt ??= GetComponent<PlayerTransform>();
        pa ??= GetComponentInChildren<PlayerAni>();
        mm ??= GetComponentInChildren<MotionManager>();
        cb ??= GetComponent<CombatBase>();
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnding) return;
        if (ps.GetState(StateType.IsControlled)) return;

        if (Input.GetAxis("Vertical") != 0)
            moveDir = Input.GetAxis("Vertical") > 0 ? 1 : 2;

        if (Input.GetAxis("Horizontal") != 0)
            moveDir = Input.GetAxis("Horizontal") > 0 ? 4 : 3;

        if (GetKey())
            moveDir = 0;
        else if(moveDir != 0)
            move();
            
    }

	private void Initialize()
	{
        inputMap = new Dictionary<InputType, string>
        {
            { InputType.Run, "move_fight_R" },
            { InputType.Walk, "move" },
            { InputType.SwitchPeace, "switch_peace" },
            { InputType.SwitchFight, "switch_fight_R" },
            { InputType.Block, "block_R" },
            { InputType.AttackLD, "combat_LD" },
            { InputType.AttackLU, "combat_LU" },
            { InputType.AttackRD, "combat_RD" },
            { InputType.AttackRU, "combat_RU" },
            { InputType.SkillVerticeReady, "skill_vertical_ready" },
            { InputType.SkillVerticeCast, "skill_vertical_cast" },
        };
	}

    bool GetKey()
	{
        foreach(InputType it in Enum.GetValues(typeof(InputType)))
		{
            if (it < 0) continue;
			if (Input.GetKeyDown((KeyCode)it))
			{
                switch (it)
				{
                    case InputType.TriggerUI:
                        ui.TriggerPauseUI();
                        break;
                    default:
                        if (ui.isPausing) ui.TriggerPauseUI(false);
                        if (DoMotion(inputMap[it])) 
                            return true;
                        break;
				}
			}
		}
        return false;
	}

	private void move()
	{
        if (ui.isPausing) ui.TriggerPauseUI(false);
        if (ps.GetState(StateType.IsFighting))
            DoMotion(inputMap[InputType.Run]);
        else
            DoMotion(inputMap[InputType.Walk]);

        moveDir = 0;
    }

    public bool DoMotion(string name, int? dir = null)
    {
        if (isEnding) return false;
        return mm.DoMotion(name, dir ?? moveDir);
	} 

   

    public void ControlledEffect(Attack atk)
	{
        if (ui.isPausing) ui.TriggerPauseUI();
        mm.InterruptMotion();
	}

    public void End(bool isWin)
	{
        isEnding = true;
        mm.InterruptMotion();
        pt.StopMove();
		if (isWin)
		{
            // Play victory sound
		}
		else
		{
            //play loss sound
		}
	} 
    public void InjuredEffect(Attack atk)
	{
        //Play injured sound
        am.Play("injured");
	}

    public void HitEffect(Attack atk, CombatBase cb)
	{
        //Play hit with sword sound
        am.Play("attack");
    }
}
