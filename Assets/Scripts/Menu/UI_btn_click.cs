using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;



public class UI_btn_click : MonoBehaviour
{
    private Animator aniGame, aniInit;
    bool PauseFlag = true;
    float timescale_record = 1;
    bool cangetInput = true;
    bool pausing = false;

    DateTime startTime;
    DateTime ctime;

    private bool timerStarted = false;

    private void Awake()
    {
        aniGame = GameObject.Find("gaming_UI_canvas").GetComponent<Animator>();
        aniInit = GameObject.Find("initial_UI_canvas").GetComponent<Animator>();
    }

    //設定timescale = 0
    public void timeStop()
    {
        if(Time.timeScale != 0)
            timescale_record = Time.timeScale;
        Time.timeScale = 0;
    }

    //恢復timescale = 1
    public void timerecovery()
    {
        Time.timeScale = timescale_record;
    }

    //"暫停" 觸發的動畫處理
    public void PlayopenMenuAi()
    {
        aniGame.SetBool("pause", true);
        Invoke("timeStop", 1.1f);
    }

    //"繼續" 觸發的動畫處理
    public void PlayContinueAi()
    {
        if (PauseFlag)
        {
            timerecovery();
            aniGame.SetBool("Continue_Button", true);
            PauseFlag = false;
        }
    }

    public void PlayRestartAi()
    {
        if (PauseFlag)
        {
            timerecovery();
            aniGame.SetBool("Restart_Button", true);
            StartCoroutine(toPage_initial("initial_UI_canvas"));
            //toPage_initial("initial_UI_canvas");
            PauseFlag = false;
        }
    }

    //切換到初始畫面
    IEnumerator toPage_initial(string pageName)
    {
        Menu._ins.switchUIPage(pageName);
        yield return new WaitForSeconds(1);
        timeStop();
    }

    //切換到遊戲畫面
    public void toPage_gaming(string pageName)
    {
        timerecovery();
        Menu._ins.switchUIPage(pageName);
    }

    //用於避免如果連續觸發造成問題
    public void timeSet()
    {
        startTime = DateTime.Now;
        timerStarted = true;
        cangetInput = false;
    }

    void Update()
    {
        //設定2秒後才會在接收觸發
        if (timerStarted && (DateTime.Now - startTime).TotalSeconds >= 2)
        {
            timerStarted = false;
            cangetInput = true;
        }

        //檢查有無觸發
        if(cangetInput)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //pausing用途: 避免暫停狀態，觸發這裡就變成還暫停狀態，timeScale變成1
                if(!pausing)
                {
                    timeSet();
                    toPage_gaming("gaming_UI_canvas");
                }
            }

            // 暫停按鍵
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                pausing = true;
                timeSet();
                PlayopenMenuAi();
                PauseFlag = true;
            }

            //繼續按鍵
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                pausing = false;
                timeSet();
                PlayContinueAi();
            }

            // 重新開始按鍵
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                pausing = false;
                timeSet();
                PlayRestartAi();
            }
        }
    }
}
