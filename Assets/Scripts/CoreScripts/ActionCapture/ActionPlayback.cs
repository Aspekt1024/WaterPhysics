using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPlayback : MonoBehaviour {

    private GameController gameController;
    private ActionCapture actionCapture;
    private List<ActionType> actions = new List<ActionType>();
    private int actionIndex;
    private bool isPlayingBack;
    private float startTime;

    private static ActionPlayback actionPlayback;
    private static ActionPlayback current
    {
        get { return actionPlayback; }
    }

    private void Start()
    {
        actionPlayback = this;
        gameController = GetComponent<GameController>();
        actionCapture = GetComponent<ActionCapture>();
        if (gameController == null || actionCapture == null)
        {
            Debug.LogError("ActionPlayback must be on the same GameObject as GameController and ActionCapture");
        }
    }

    private void Update ()
    {
        if (!isPlayingBack) return;
        
        CallMethodIfTime();
	}
    
    public static void PlayBack(string sessionName)
    {
        current.startTime = Time.time;
        current.actions = ActionSerializer.Load(current.actionCapture.SessionName).Actions;
        current.isPlayingBack = true;
        current.actionIndex = 0;
        current.gameController.SetPlaybackMode();
    }

    private void CallMethodIfTime()
    {
        if (actions[actionIndex].Time > Time.time - startTime) return;

        if (actions[actionIndex].HasMode)
        {
            gameController.SendMessage(actions[actionIndex].MethodName, actions[actionIndex].Mode);
        }
        else
        {
            gameController.SendMessage(actions[actionIndex].MethodName);
        }
        actionIndex++;

        if (actionIndex == actions.Count)
        {
            isPlayingBack = false;
            gameController.SetPlayingMode();
        }
    }
}
