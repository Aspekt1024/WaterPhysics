using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public Submarine Submarine;
    public Camera MainCamera;

    private enum States
    {
        None, Playing, Waiting, Paused
    }
    private States state;

    private InputHandler inputHandler;

    #region LifeCycle
    private void Start ()
    {
        inputHandler = new InputHandler(this);
        state = States.Playing;
	}
	
	private void Update ()
    {
		switch(state)
        {
            case States.None:
                break;
            case States.Playing:
                inputHandler.ProcessInput();
                break;
            case States.Waiting:
                break;
            case States.Paused:
                break;
        }
	}
    #endregion

    #region PlayerInputs
    public void TurnLeftPressed()
    {
        if (state != States.Playing) return;
        Submarine.TurnLeft();
    }

    public void TurnRightPressed()
    {
        if (state != States.Playing) return;
        Submarine.TurnRight();
    }

    public void AcceleratePressed()
    {
        if (state != States.Playing) return;
        Submarine.Accelerate();
    }

    public void FloatModePressed() { Submarine.SetFloatMode(); }
    public void PeriscopeModePressed() { Submarine.SetPeriscopeMode(); }
    public void SubmergedModePressed() { Submarine.SetSubmergedMode(); }
    public void SpecialPressed() { Submarine.ToggleSpecial(); }

    public void ToggleCameraFollowPressed()
    {
        CameraFollow camFollow = MainCamera.GetComponent<CameraFollow>();
        camFollow.enabled = !camFollow.enabled;
    }
    #endregion
}
