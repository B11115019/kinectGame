using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MotionManager : MonoBehaviour
{
    public MotionBase[] motions;
    public UIManager ui;

    MotionBase CurMotion = null;
    // Start is called before the first frame update
    void Start()
    {
        GetMotions();
    }

    public MotionBase GetMotion(string name)
	{
        foreach(MotionBase m in motions)
		{
            if (m.AniName == name)
                return m;
		}
        return null;
	}

    void GetMotions()
	{
        motions = transform.parent.GetComponentsInChildren<MotionBase>(true);	
        foreach(MotionBase m in motions)
		{
            m.SetMotionManager(this);
		}
	}

    public void TriggerEvent(string info = null)
	{
        CurMotion?.AniEvent(info);
	}
    
    public bool DoMotion(string name, int dir = 1)
	{
        if (name == null) return false;
        CurMotion = motions.FirstOrDefault(m => m.AniName == name && m.Do(dir, CurMotion)) ?? CurMotion;
        return CurMotion?.AniName == name;
	}
    
    public void MotionEnd(string name)
	{
        if(name == CurMotion?.AniName)
            CurMotion = null;
	}

    public void InterruptMotion()
	{
        if (CurMotion == null) return;
        CurMotion.Interrupt(-1, 0);
        CurMotion = null;
	}

    public void ShowEnd()
    {
        ui.ShowEndAndSwitch();
    }
}
