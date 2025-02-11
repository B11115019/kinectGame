using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Enums;

namespace Enums
{
    public enum AniTrigger
    {
        Switch,
        Attack,
        Block,
        MoveInFight,
        Contrlled,
        SkillReady,
        SkillCast,
        Stay,
    }

    public enum AniInt
	{
        AttackMode,
        ControlledMode,
	}
}

[RequireComponent(typeof(Animator))]
public class PlayerAni : MonoBehaviour
{
    public Animator ani;
    AnimationClip[] clips;
    [SerializeField] GameObject weapon_back, weapon_r;
    // Start is called before the first frame update

    PlayerEventTrigger et;
    PlayerState ps;

    Dictionary<AniTrigger, string> triggerMap = new Dictionary<AniTrigger, string>
    {
        { AniTrigger.Switch, "Switch" },
        { AniTrigger.Attack, "Attack" },
        { AniTrigger.MoveInFight, "MoveInFight" },
        { AniTrigger.Block, "Block" },
        { AniTrigger.Contrlled, "Controlled" },
        { AniTrigger.SkillReady, "SkillReady" },
        { AniTrigger.SkillCast, "SkillCast" },
        { AniTrigger.Stay, "Stay" },
    };

    Dictionary<AniInt, string> intMap = new Dictionary<AniInt, string>
    {
        { AniInt.AttackMode, "AttackMode"},
        { AniInt.ControlledMode, "ControlMode"}        
    };

    public float walkTime;

    void Start()
    {
        ani = GetComponent<Animator>();
        et = GetComponentInParent<PlayerEventTrigger>();
        ps = GetComponentInParent<PlayerState>();
        initialize();
        SwitchWeapon();
    }

    void initialize()
    {
        clips = ani.runtimeAnimatorController.animationClips;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchWeapon()
    {
        bool inFight = ps.GetState(StateType.IsFighting);
        weapon_r.SetActive(inFight);
        weapon_back.SetActive(!inFight);
    }

    public void PlayAni(string name)
	{
        ani.Play(name, 0, 0);
	}

    public void SetBool(Dictionary<string, bool> dict)
	{
        if (dict == null) return;
        foreach (KeyValuePair<string, bool> pair in dict)
        {
            ani.SetBool(pair.Key, pair.Value);
        }
	}

    public void SetBool(string name, bool val)
	{
        ani.SetBool(name, val);
	}

    public void SetTrigger(AniTrigger at)
	{
        ani.SetTrigger(triggerMap[at]);
	}

    public void SetInt(AniInt i, int value)
	{
        ani.SetInteger(intMap[i], value);
	}
    
}
