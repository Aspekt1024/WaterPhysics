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
    public KeyCode SET_EXIT_SCENE1 = KeyCode.Keypad3;
    public KeyCode SET_EXIT_SCENE2 = KeyCode.Keypad3;
    public KeyCode SET_EXIT_SCENE3 = KeyCode.Keypad3;
    public KeyCode SET_FALLING_SCENE = KeyCode.Keypad7;

    private GameController gameController;

    public InputHandler(GameController controller)
    {
        gameController = controller;
    }

    public void ProcessInput()
    {
        if (Input.GetKey(TURN_LEFT)) gameController.TurnLeftPressed();
        if (Input.GetKey(TURN_RIGHT)) gameController.TurnRightPressed();

        if (Input.GetKey(ACCELERATE))
        {
            if (Input.GetKey(LEFT_SHIFT)) gameController.TurboAcceleratePressed();
            else gameController.AcceleratePressed();
        }

        if (Input.GetKeyDown(SET_FLOAT_MODE)) gameController.FloatModePressed();
        if (Input.GetKeyDown(SET_PERISCOPE_MODE)) gameController.PeriscopeModePressed();
        if (Input.GetKeyDown(SET_SUBMERGED_MODE)) gameController.SubmergedModePressed();

        if (Input.GetKeyDown(SPECIAL)) gameController.SpecialPressed();
        if (Input.GetKeyDown(TOGGLE_CAMERA_FOLLOW)) gameController.ToggleCameraFollowPressed();

        if (Input.GetKeyDown(SET_ENTRANCE_SCENE1)) gameController.SetGameStart(SceneDirectorManager.StartModes.Entrance1);
        if (Input.GetKeyDown(SET_ENTRANCE_SCENE2)) gameController.SetGameStart(SceneDirectorManager.StartModes.Entrance2);
        if (Input.GetKeyDown(SET_ENTRANCE_SCENE3)) gameController.SetGameStart(SceneDirectorManager.StartModes.Entrance2);
        if (Input.GetKeyDown(SET_EXIT_SCENE1)) gameController.SetGameStart(SceneDirectorManager.StartModes.Exit1);
        if (Input.GetKeyDown(SET_EXIT_SCENE2)) gameController.SetGameStart(SceneDirectorManager.StartModes.Exit2);
        if (Input.GetKeyDown(SET_EXIT_SCENE3)) gameController.SetGameStart(SceneDirectorManager.StartModes.Exit3);
        if (Input.GetKeyDown(SET_FALLING_SCENE)) gameController.SetGameStart(SceneDirectorManager.StartModes.Falling);
    }

    public bool UnpausedPressed()
    {
        return Input.GetKeyDown(SPECIAL);
    }
}
