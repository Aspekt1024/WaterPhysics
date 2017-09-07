using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public Submarine Submarine;
    public Camera MainCamera;

    private enum States
    {
        None, Playing, Waiting, Paused, Unpausing, Pausing
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
            case States.Unpausing:
                Submarine.UnPause();
                state = States.Playing;
                break;
            case States.Pausing:
                Submarine.SetPaused();
                state = States.Paused;
                break;
            case States.Playing:
                inputHandler.ProcessInput();
                break;
            case States.Waiting:
                break;
            case States.Paused:
                if (inputHandler.UnpausedPressed())
                {
                    state = States.Unpausing;
                }
                break;
        }
	}
    #endregion

    #region GameSetup
    public void SetGameStart(SceneDirectorManager.StartModes mode)
    {
        SceneDirectorManager.Instance.ResetToMode(mode);
        state = States.Pausing;
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

    public void TurboAcceleratePressed()
    {
        if (state != States.Playing) return;
        Submarine.TurboAccelerate();
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
