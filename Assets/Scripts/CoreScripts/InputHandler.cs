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
    public KeyCode SPECIAL = KeyCode.Space;
    public KeyCode TOGGLE_CAMERA_FOLLOW = KeyCode.C;

    private GameController gameController;

    public InputHandler(GameController controller)
    {
        gameController = controller;
    }

    public void ProcessInput()
    {
        if (Input.GetKey(TURN_LEFT)) gameController.TurnLeftPressed();
        if (Input.GetKey(TURN_RIGHT)) gameController.TurnRightPressed();
        if (Input.GetKey(ACCELERATE)) gameController.AcceleratePressed();

        if (Input.GetKeyDown(SET_FLOAT_MODE)) gameController.FloatModePressed();
        if (Input.GetKeyDown(SET_PERISCOPE_MODE)) gameController.PeriscopeModePressed();
        if (Input.GetKeyDown(SET_SUBMERGED_MODE)) gameController.SubmergedModePressed();

        if (Input.GetKeyDown(SPECIAL)) gameController.SpecialPressed();

        if (Input.GetKeyDown(TOGGLE_CAMERA_FOLLOW)) gameController.ToggleCameraFollowPressed();
    }
}
