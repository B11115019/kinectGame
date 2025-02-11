using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        InputManager.AllowInput = false;
        StartCoroutine(AllowTimer());
    }

    IEnumerator AllowTimer()
	{
        yield return null;
        yield return null;
        InputManager.AllowInput = true;
	}
}
