using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
public class MonsterHealthUI : HealthUIBase
{
    public GameObject target;
    public GameObject HpBarObj;
    public Slider hpBar;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target.transform);
    }

	public override void Initialize()
	{
        base.Initialize();
        target ??= GameObject.FindWithTag("Player");
        hpBar ??= HpBarObj.GetComponent<Slider>();
	}

	public override void HealthUpdate(AbilityType h)
	{
		base.HealthUpdate(h);
        switch (h)
		{
            case AbilityType.Hp:
                UpdateHp();
                break;
		}
	}

    void UpdateHp()
	{
        hpBar.value =  Healths[AbilityType.Hp].First/Healths[AbilityType.Hp].Second;
    }
}
