using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class PlayerHealthUI : HealthUIBase
{
    public GameObject hpL, hpR, hpT, mpL, mpR, mpT, skCT;
    Image hpLi, hpRi, mpLi, mpRi;
    Text hpTt, mpTt, skCTt;
    public GameObject[] Sps;
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

	public override void Initialize()
	{
        base.Initialize();
        hpLi = hpL.GetComponent<Image>();
        hpRi = hpR.GetComponent<Image>();
        mpLi = mpL.GetComponent<Image>();
        mpRi = mpR.GetComponent<Image>();
        hpTt = hpT.GetComponent<Text>();
        mpTt = mpT.GetComponent<Text>();
        skCTt = skCT.GetComponent<Text>();
    }


	public override void HealthUpdate(AbilityType h)
    {
		switch (h)
		{
            case AbilityType.Hp:
                UpdateHp();
                break;
            case AbilityType.Mp:
                UpdateMp();
                break;
            case AbilityType.Sp:
                UpdateSp();
                break;
            case AbilityType.SkillCompletion:
                UpdateSkillCompletion();
                break;
		}
    }

    void UpdateHp()
    {
        Pair<float, float> p = GetFillAmount(
            Healths[AbilityType.Hp].First, Healths[AbilityType.Hp].Second);
        hpLi.fillAmount = p.First;
        hpRi.fillAmount = p.Second;

        UpdateHpText(Healths[AbilityType.Hp].First, Healths[AbilityType.Hp].Second);
    }

    void UpdateHpText(float cur, float max)
	{
        hpTt.text = Mathf.Clamp(cur, 0, max).ToString();
	}

    void UpdateMp()
    {
        Pair<float, float> p = GetFillAmount(
            Healths[AbilityType.Mp].First, Healths[AbilityType.Mp].Second);
        mpLi.fillAmount = p.First;
        mpRi.fillAmount = p.Second;
        UpdateMpText(Healths[AbilityType.Mp].First, Healths[AbilityType.Mp].Second);
    }

    void UpdateMpText(float cur, float max)
	{
        mpTt.text = Mathf.Clamp(cur, 0, max).ToString();
    }

	private void UpdateSp()
	{
        Pair<float, float> p = GetFillAmount(
            Healths[AbilityType.Sp].First, Healths[AbilityType.Sp].Second);
        int i = (int)(Sps.Length * (p.First + p.Second) * 0.5) - 1;
        for(int j = 0; j < Sps.Length; j++)
            Sps[j].SetActive(j <= i);
    }

    Pair<float, float> GetFillAmount(float m, float d)
    {
        if (d <= 0)
            return new Pair<float, float>(0, 0);
        if (m <= d / 2)
            return new Pair<float, float>((m + m) / d, 0);
        else
            return new Pair<float, float>(1, (m + m) / d - 1);
    }

    void UpdateSkillCompletion()
	{
        skCTt.text = ((int)(Mathf.Clamp(Healths[AbilityType.SkillCompletion].First, 0, Healths[AbilityType.SkillCompletion].Second))).ToString();
    }
}
