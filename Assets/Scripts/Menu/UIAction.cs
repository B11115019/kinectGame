using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using UnityEditor;

public class UIAction : MonoBehaviour
{
    private Animator aniGame;
    float timescale_record = 1;

    bool cangetInput = true;
    bool pausing = false;
    bool isGaming = false;

    DateTime StartTime;

    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;

    private void Awake()
    {
        aniGame = GameObject.Find("gaming_UI_canvas").GetComponent<Animator>();
        winText = GameObject.Find("WinText").GetComponent<TextMeshProUGUI>();
        loseText = GameObject.Find("LoseText").GetComponent<TextMeshProUGUI>();
    }

    //設定timescale = 0
    public void timeStop()
    {
        if (Time.timeScale != 0)
            timescale_record = Time.timeScale;
        Time.timeScale = 0;
    }

    //恢復timescale = 1
    public void timerecovery()
    {
        Time.timeScale = timescale_record;
    }

    //切換到初始畫面
    public void toPage_initial()
    {
        Menu._ins.switchUIPage("initial_UI_canvas");
        timeStop();
    }

    //切換到遊戲畫面
    public void toPage_gaming()
    {
        Delay();

        if (!isGaming)
        {
            if (cangetInput)
            {
                isGaming = true;
                cangetInput = false;
                StartTime = DateTime.Now;

                timerecovery();
                Menu._ins.switchUIPage("gaming_UI_canvas");
            }
        }
    }

    //"暫停"
    public void PlayopenMenuAi()
    {
        Delay();

        if (cangetInput)
        {
            pausing = true;
            cangetInput = false;
            StartTime = DateTime.Now;
            aniGame.SetBool("pause", true);
            Invoke("timeStop", 1.3f);
        }
    }

    //"繼續"
    public void PlayContinueAi()
    {
        Delay();

        if (cangetInput)
        {
            cangetInput = false;
            StartTime = DateTime.Now;

            if (pausing && isGaming)
            {
                timerecovery();
                aniGame.SetBool("Continue_Button", true);
                pausing = false;
            }
        }
    }

    //重新開始
    public void PlayRestartAi()
    {
        Delay();

        if (cangetInput)
        {
            cangetInput = false;
            StartTime = DateTime.Now;

            if (pausing && isGaming)
            {
                timerecovery();
                Invoke("toPage_initial", 0.1f);
                aniGame.SetBool("Restart_Button", true);
                pausing = false;
                isGaming = false;
            }
        }
    }

    //顯示勝利
    public void GameOver_Win()
    {
        backToInitialDelay();

        if (cangetInput && !pausing)
        {
            StartTime = DateTime.Now;
            cangetInput = false;

            Menu._ins.switchUIPage("ending_UI_canvas");

            loseText.gameObject.SetActive(false);
            isGaming = false;

            StartCoroutine(WaitAndSwitch("initial_UI_canvas"));
        }
    }

    //顯示失敗
    public void GameOver_Lose()
    {
        backToInitialDelay();

        if (cangetInput && !pausing)
        {
            StartTime = DateTime.Now;
            cangetInput = false;

            Menu._ins.switchUIPage("ending_UI_canvas");

            winText.gameObject.SetActive(false);

            StartCoroutine(WaitAndSwitch("initial_UI_canvas"));
        }
    }

    private IEnumerator WaitAndSwitch(string pageName)
    {
        yield return new WaitForSeconds(3f);
        isGaming = false;
        timeStop();
        Menu._ins.switchUIPage(pageName);

        //恢復預設
        winText.gameObject.SetActive(true);
        loseText.gameObject.SetActive(true);
    }

    public void Delay()
    {
        //設定2秒後才會在接收觸發
        if ((DateTime.Now - StartTime).TotalSeconds >= 2)
        {
            cangetInput = true;
        }
    }

    public void backToInitialDelay()
    {
        //設定2秒後才會在接收觸發
        if ((DateTime.Now - StartTime).TotalSeconds >= 5)
        {
            cangetInput = true;
        }
    }

    //test
    //public void Update()
    //{
    //    Debug.Log("cangetInput = " + cangetInput);
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        toPage_gaming();
    //    }
    //    if (Input.GetKeyDown(KeyCode.UpArrow))
    //    {
    //        PlayopenMenuAi();
    //    }
    //    if (Input.GetKeyDown(KeyCode.DownArrow))
    //    {
    //        PlayContinueAi();
    //    }
    //    if (Input.GetKeyDown(KeyCode.LeftArrow))
    //    {
    //        PlayRestartAi();
    //    }
    //    if (Input.GetKeyDown(KeyCode.RightArrow))
    //    {
    //        GameOver_Win();
    //    }
    //    if (Input.GetKeyDown(KeyCode.H))
    //    {
    //        GameOver_Lose();
    //    }
    //}
}
