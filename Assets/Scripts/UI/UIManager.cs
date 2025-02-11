using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas pauseUIC, endUIC;
	public TMPro.TextMeshProUGUI EndText;
	public float SwitchSceneTime = 3f;
	public float TriggerInterval = 0.5f;

    public bool isDied = false;
    public bool isPausing = false;

	bool canTrigger = true;
	float oriTimeScale;

	void Start()
	{
		pauseUIC.enabled = false;
		endUIC.enabled = false;
		Ability.DiedEvent += TriggerDiedUI;
	}

	public void TriggerDiedUI(bool isWin = true)
	{
        InputManager.AllowInput = false;
        if (isPausing) ClosePauseUI();
        Time.timeScale = 1;
        isDied = true;
        EndText.text = isWin ? "You Win!" : "You Lose~~";
		if (!isWin) ShowEndAndSwitch();
    }

	public void ShowEndAndSwitch()
	{
		endUIC.enabled = true;
		CancelInvoke("TriggerTimer");
		Invoke("SwitchScene", SwitchSceneTime);
	}
	
	public void SwitchScene()
	{
		Loader.Load(Loader.Scene.Begin);
	}

	public void TriggerPauseUI(bool needInterval = true)
	{
		if (isDied || ! canTrigger) return;
		if (isPausing)
			ClosePauseUI();
		else
			OpenPauseUI();
		if (needInterval)
		{
			canTrigger = false;
			Invoke("TriggerTimer", TriggerInterval);
		}
		
	}

	public void TriggerTimer()
	{
		canTrigger = true;
	}

    void OpenPauseUI()
	{
		isPausing = true;
		pauseUIC.enabled = true;
		//oriTimeScale = Time.timeScale;
		//Time.timeScale = 0.1f;
	}

	void ClosePauseUI()
	{
		isPausing = false;
		pauseUIC.enabled = false;
		//Time.timeScale = oriTimeScale;
	}
}
