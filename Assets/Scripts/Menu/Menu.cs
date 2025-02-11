using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{
    public static Menu _ins;
    public static bool OpenBeginMenu = true;

    private GameObject[] allPages;

    void Awake()
    {
        _ins = this;
        allPages = GameObject.FindGameObjectsWithTag("initial_UI_tag");
    }

    public void switchUIPage(string pageName)
    {
        foreach(GameObject page in allPages)
        {
            if(page.name == pageName)
            {
                page.GetComponent<Canvas>().enabled = true;
            }
            else
            {
                page.GetComponent<Canvas>().enabled = false;
            }
        }
    }

    void Start()
    {
		if (OpenBeginMenu)
		{
            OpenBeginMenu = false;
            Time.timeScale = 0;
            switchUIPage("initial_UI_canvas");
		}
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.M))
		{
            Loader.Load(Loader.Scene.GameScene);
		}
	}
}
