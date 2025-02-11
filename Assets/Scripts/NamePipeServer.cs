using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(UnityMainThreadDispatcher))]

public class NamedPipeServer : MonoBehaviour
{
    private NamedPipeServerStream pipeServer;
    private StreamReader reader;
    private bool isRunning = true;
    private Dictionary<string, DateTime> lastActionTime = new Dictionary<string, DateTime>();
    private const float actionThreshold = 2.0f; // 2 seconds

    private async void Awake()
    {
        // 使用 Task.Run 执行异步的命名管道服务器启动
        await Task.Run(() => StartNamedPipeServer());
    }

    private void StartNamedPipeServer()
    {
        while (isRunning)
        {
            try
            {
                pipeServer = new NamedPipeServerStream("UnityPipe", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                pipeServer.WaitForConnection();
                reader = new StreamReader(pipeServer);

                while (pipeServer.IsConnected)
                {
                    var message = reader.ReadLine();
                    if (message != null)
                    {
                        DateTime currentTime = DateTime.Now;
                        if (lastActionTime.ContainsKey(message))
                        {
                            double elapsedSeconds = (currentTime - lastActionTime[message]).TotalSeconds;
                            if (elapsedSeconds < actionThreshold)
                            {
                                Debug.Log("Shake detected for action: " + message);
                                // Handle shake detection if necessary
                                continue;
                            }
                        }

                        lastActionTime[message] = currentTime;
                        UnityMainThreadDispatcher.Instance.Enqueue(() =>
                        {
                            control(message);
                        });
                    }
                }
            }
            catch (IOException e)
            {
                UnityMainThreadDispatcher.Instance.Enqueue(() =>
                {
                    Debug.LogError("Pipe server IO exception: " + e.Message);
                });
            }
            finally
            {
                reader?.Dispose();
                pipeServer?.Dispose();
            }
        }
    }

    private void control(string message)
    {
        Debug.Log("Received from WPF: " + message);


        switch (message)
        {
            // 移動
            case "mf":
                InputManager.GetInput(Enums.InputType.Walk, 1);
                break;
            case "mb":
                InputManager.GetInput(Enums.InputType.Walk, 2);
                break;
            case "ml":
                InputManager.GetInput(Enums.InputType.Walk, 3);
                break;
            case "mr":
                InputManager.GetInput(Enums.InputType.Walk, 4);
                break;

            // 手勢
            case "hd":
                if (InputManager.pc?.ps.GetState(Enums.StateType.IsSkillReading) == true)
                    InputManager.GetInput(Enums.InputType.SkillVerticeCast, 0);
                break;
            case "hu":
                if (InputManager.pc?.ps.GetState(Enums.StateType.IsSkillReading) == true)
                    InputManager.GetInput(Enums.InputType.SkillVerticeCast, 0);
                break;
            case "hr":
            case "hl":
                if(Loader.CurScene == Loader.Scene.Begin)
                    InputManager.GetInput(Enums.InputType.Begin, 4);
                else
                    InputManager.GetInput(Enums.InputType.TriggerUI, 4);

                break;

            // 特殊動作
            case "block":
                InputManager.GetInput(Enums.InputType.Block, 0);
                break;

            case "take":
                if(InputManager.pc?.ps.GetState(Enums.StateType.IsFighting) == true)
                    InputManager.GetInput(Enums.InputType.SkillVerticeReady, 0);
                else
                    InputManager.GetInput(Enums.InputType.SwitchFight, 0);
                    
                break;
            case "s1":
                InputManager.GetInput(Enums.InputType.SkillVerticeReady);
                break;
            case "s2":
            // 攻擊
            case "ldk":
                if (InputManager.pc?.ps.GetState(Enums.StateType.IsSkillReading) == true)
                    InputManager.GetInput(Enums.InputType.SkillVerticeCast, 0);
                else
                    InputManager.GetInput(Enums.InputType.AttackRU, 0);
                break;
            case "rdk":
                if (InputManager.pc?.ps.GetState(Enums.StateType.IsSkillReading) == true)
                    InputManager.GetInput(Enums.InputType.SkillVerticeCast, 0);
                else
                    InputManager.GetInput(Enums.InputType.AttackLU, 0);
                break;
            case "ruk":
                if (InputManager.pc?.ps.GetState(Enums.StateType.IsSkillReading) == true)
                    InputManager.GetInput(Enums.InputType.SkillVerticeCast, 0);
                else
                    InputManager.GetInput(Enums.InputType.AttackLD, 0);
                break;
            case "luk":
                if (InputManager.pc?.ps.GetState(Enums.StateType.IsSkillReading) == true)
                    InputManager.GetInput(Enums.InputType.SkillVerticeCast, 0);
                else
                    InputManager.GetInput(Enums.InputType.AttackRD, 0);
                break;

        }
    }

    private void OnDestroy()
    {
        isRunning = false;
        reader?.Dispose();
        pipeServer?.Dispose();
    }
}