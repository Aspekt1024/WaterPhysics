using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ActionContainer
{
    public List<ActionType> Actions;

    public ActionContainer()
    {
         Actions = new List<ActionType>();
    }
}

[Serializable]
public class ActionType
{
    public string MethodName;
    public float Time;
    public bool HasMode;
    public SceneDirectorManager.StartModes Mode;

    public ActionType()
    {
        MethodName = "";
        Time = 0f;
        HasMode = false;
        Mode = SceneDirectorManager.StartModes.Entrance1;
    }
}