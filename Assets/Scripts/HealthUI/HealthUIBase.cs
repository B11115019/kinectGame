using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class HealthUIBase : MonoBehaviour
{
    

    public Dictionary<AbilityType, Pair<float, float>> Healths = new Dictionary<AbilityType, Pair<float, float>>();// cur / max
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
    }

    virtual public void Initialize() 
    {
        Invoke("HealthUpdateAll", 0);
    }

    virtual public void HealthChange(AbilityType h, float cur)
	{
        if (!Healths.ContainsKey(h)) return;
        Healths[h].First = cur;
        HealthUpdate(h);
    }

    virtual public void IniHealth(AbilityType h, float cur, float max)
	{
        max = Mathf.Max(0, max);
        Healths[h] = new Pair<float, float>(cur < 0 ? max : cur, max);
	}

    virtual public void HealthUpdate(AbilityType h)
	{
	}

    virtual public void HealthUpdateAll()
	{
        foreach (KeyValuePair<AbilityType, Pair<float, float>> pair in Healths)
            HealthUpdate(pair.Key);
	}

}

public class Pair<T, U>
{
    public Pair()
    {
    }

    public Pair(T first, U second)
    {
        this.First = first;
        this.Second = second;
    }

    public T First { get; set; }
    public U Second { get; set; }
};