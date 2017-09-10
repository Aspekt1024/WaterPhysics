using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCapture : MonoBehaviour
{
    public string SessionName;

    private bool isRecording;
    private float startTime;
    private List<ActionType> actions = new List<ActionType>();

    private static ActionCapture actionCapture;
    private static ActionCapture instance
    {
        get
        {
            if (actionCapture == null)
            {
                Debug.LogError("No instances of ActionCapture found. Please add one to the scene.");
                return null;
            }
            return actionCapture;
        }
    }

    private void Awake()
    {
        if (actionCapture != null)
        {
            Debug.LogError("Multiple instances of ActionCapture found in scene. Only one should exist.");
            return;
        }
        actionCapture = this;
    }

    public static void AddAction(string methodName, float time, bool hasMode = false, SceneDirectorManager.StartModes mode = SceneDirectorManager.StartModes.Entrance1)
    {
        if (!instance.isRecording) return;
        ActionType newAction = new ActionType()
        {
            MethodName = methodName,
            Time = time - instance.startTime,
            HasMode = hasMode,
            Mode = mode
        };
        instance.actions.Add(newAction);
    }

    public static void Save(string sessionName)
    {
        ActionSerializer.Save(instance.actions, sessionName);
    }

    public static bool Load()
    {
        ActionContainer actionData = ActionSerializer.Load(instance.SessionName);
        instance.actions = actionData.Actions;
        if (instance.actions == null || instance.actions.Count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static void StartRecording()
    {
        instance.actions = new List<ActionType>();
        instance.startTime = Time.time;
        instance.isRecording = true;
    }

    public static void StopRecording()
    {
        instance.isRecording = false;
    }

    public static void Playback()
    {
        Debug.Log("playing back");
        instance.isRecording = false;
        ActionPlayback.PlayBack(instance.SessionName);
    }
}
