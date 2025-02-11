using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    public float RefreshInterval = 10;

    void Start()
    {
        Invoke("Timer", 0);
    }

    public void Timer()
    {
        Loader.LoaderCallback();
        Invoke("Timer", 10);
    }
}
