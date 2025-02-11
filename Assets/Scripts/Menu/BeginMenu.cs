using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class BeginMenu : MonoBehaviour
{
	private void Start()
	{
		InputManager.bm = this;
	}
	private void Update()
	{
		if (Input.GetKey((KeyCode)InputType.Begin))
			DoMotion(InputType.Begin);
	}

	public void DoMotion(InputType it)
	{
		if (it == InputType.Begin)
			Loader.Load(Loader.Scene.GameScene);
	}
}
