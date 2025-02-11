using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using UnityEngine.SceneManagement;

public static class InputManager
{
    public static BeginMenu bm;
    public static PlayerController pc;

    public static bool AllowInput = false;
    /*
	public static void SceneChange()
	{
		switch (Loader.CurScene)
		{
			case Loader.Scene.Begin:
				bm = GameObject.FindGameObjectWithTag("BeginMenu").GetComponent<BeginMenu>();
				if (bm == null)
					Debug.Log("bm not found");
				break;
			case Loader.Scene.GameScene:
				pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
				if (pc == null) 
					Debug.Log("pc not found");
				break;
		}
	}*/

    // cross all scene and safe to use
    public static void GetInput(InputType it, int? dir = null)
    {
        if (!AllowInput) return;
        switch (Loader.CurScene)
        {
            case Loader.Scene.GameScene:
                if (pc == null || pc.isEnding) return;
                if (it == InputType.Walk || it == InputType.Run)
                    it = pc.ps.GetState(StateType.IsFighting) ? InputType.Run : InputType.Walk;
                if (it == InputType.TriggerUI && pc?.ps.GetState(StateType.IsFighting) == false)
                {
                    pc.ui.TriggerPauseUI();
                    break;
                }
                if (pc.inputMap.ContainsKey(it))
                    pc.DoMotion(pc.inputMap[it], dir);
                break;
            case Loader.Scene.Begin:
                bm?.DoMotion(it);
                break;
            case Loader.Scene.Loading:
                break;
        }
    }
}
