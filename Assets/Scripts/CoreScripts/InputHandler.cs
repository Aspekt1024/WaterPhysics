using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler {

    // Can use system.enum.parse to serialize these
    public KeyCode SET_FLOAT_MODE = KeyCode.Alpha1;
    public KeyCode SET_PERISCOPE_MODE = KeyCode.Alpha2;
    public KeyCode SET_SUBMERGED_MODE = KeyCode.Alpha3;
    public KeyCode TURN_LEFT = KeyCode.A;
    public KeyCode TURN_RIGHT = KeyCode.D;
    public KeyCode ACCELERATE = KeyCode.W;
    public KeyCode LEFT_SHIFT = KeyCode.LeftShift;
    public KeyCode SPECIAL = KeyCode.Space;
    public KeyCode TOGGLE_CAMERA_FOLLOW = KeyCode.C;

    public KeyCode SET_ENTRANCE_SCENE1 = KeyCode.Keypad1;
    public KeyCode SET_ENTRANCE_SCENE2 = KeyCode.Keypad2;
    public KeyCode SET_ENTRANCE_SCENE3 = KeyCode.Keypad3;
    public KeyCode SET_EXIT_SCENE1 = KeyCode.Keypad4;
    public KeyCode SET_EXIT_SCENE2 = KeyCode.Keypad5;
    public KeyCode SET_EXIT_SCENE3 = KeyCode.Keypad6;
    public KeyCode SET_FALLING_SCENE = KeyCode.Keypad7;

    private GameController gameController;
    
    public InputHandler(GameController controller)
    {
        gameController = controller;
    }

    // TODO if any of the method names change, this will break
    // Learn how to get the method name dynamically using C#4
    public void ProcessInput()
    {
        if (Input.GetKey(TURN_LEFT)) CallTrackedMethod("TurnLeftPressed");
        if (Input.GetKey(TURN_RIGHT)) CallTrackedMethod("TurnRightPressed");

        if (Input.GetKey(ACCELERATE))
        {
            if (Input.GetKey(LEFT_SHIFT)) CallTrackedMethod("TurboAcceleratePressed");
            else CallTrackedMethod("AcceleratePressed");
        }

        if (Input.GetKeyDown(SET_FLOAT_MODE)) CallTrackedMethod("FloatModePressed");
        if (Input.GetKeyDown(SET_PERISCOPE_MODE)) CallTrackedMethod("PeriscopeModePressed");
        if (Input.GetKeyDown(SET_SUBMERGED_MODE)) CallTrackedMethod("SubmergedModePressed");

        if (Input.GetKeyDown(SPECIAL)) CallTrackedMethod("SpecialPressed");
        if (Input.GetKeyDown(TOGGLE_CAMERA_FOLLOW)) CallTrackedMethod("ToggleCameraFollowPressed");

        if (Input.GetKeyDown(SET_ENTRANCE_SCENE1)) CallTrackedMethod("SetGameStart", SceneDirectorManager.StartModes.Entrance1);
        if (Input.GetKeyDown(SET_ENTRANCE_SCENE2)) CallTrackedMethod("SetGameStart", SceneDirectorManager.StartModes.Entrance2);
        if (Input.GetKeyDown(SET_ENTRANCE_SCENE3)) CallTrackedMethod("SetGameStart", SceneDirectorManager.StartModes.Entrance3);
        if (Input.GetKeyDown(SET_EXIT_SCENE1)) CallTrackedMethod("SetGameStart", SceneDirectorManager.StartModes.Exit1);
        if (Input.GetKeyDown(SET_EXIT_SCENE2)) CallTrackedMethod("SetGameStart", SceneDirectorManager.StartModes.Exit2);
        if (Input.GetKeyDown(SET_EXIT_SCENE3)) CallTrackedMethod("SetGameStart", SceneDirectorManager.StartModes.Exit3);
        if (Input.GetKeyDown(SET_FALLING_SCENE)) CallTrackedMethod("SetGameStart", SceneDirectorManager.StartModes.Falling);
    }

    public void CheckUnpausePressed()
    {
        if(Input.GetKeyDown(SPECIAL))
        {
            CallTrackedMethod("SetUnpaused");
        }
    }

    private void CallTrackedMethod(string methodName)
    {
        ActionCapture.AddAction(methodName, Time.time, hasMode:false);
        gameController.SendMessage(methodName);
    }

    private void CallTrackedMethod(string methodName, SceneDirectorManager.StartModes mode)
    {
        ActionCapture.AddAction(methodName, Time.time, hasMode:true, mode:mode);
        gameController.SendMessage(methodName, mode);
    }
}
