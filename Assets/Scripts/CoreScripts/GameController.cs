using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour {

    public Submarine Submarine;
    public Camera MainCamera;
    public MenuControl MenuControl;
    
    private enum States
    {
        None, Playing, Waiting, Paused, Unpausing, Pausing, Playback, InMenu
    }
    private States state;

    private InputHandler inputHandler;

    #region LifeCycle
    private void Start ()
    {
        inputHandler = new InputHandler(this);
        state = States.InMenu;
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
                inputHandler.CheckUnpausePressed();
                break;
            case States.Playback:
                break;
            case States.InMenu:
                break;
        }
	}
    #endregion

    #region GameSetup
    public void SetGameStart(SceneDirectorManager.StartModes mode)
    {
        SceneDirectorManager.Instance.ResetToMode(mode);
        state = States.Playing;
    }

    public void SetPlaybackMode()
    {
        state = States.Playback;
    }

    public void SetPlayingMode()
    {
        state = States.Playing;
    }

    public void SetUnpaused()
    {
        state = States.Unpausing;
    }
    #endregion

    #region PlayerInputs
    public void ShowMenuPressed()
    {
        if (MenuControl.MainMenuIsActive())
        {
            MenuControl.DisableMainMenu();
        }
        else
        {
            MenuControl.EnableMainMenu();
        }
    }

    public void TurnLeftPressed()
    {
        if (state == States.Playing || state == States.Playback)
        {
            Submarine.TurnLeft();
        }
    }

    public void TurnRightPressed()
    {
        if (state == States.Playing || state == States.Playback)
        {
            Submarine.TurnRight();
        }
    }

    public void AcceleratePressed()
    {
        if (state == States.Playing || state == States.Playback)
        {
            Submarine.Accelerate();
        }
    }

    public void TurboAcceleratePressed()
    {
        if (state == States.Playing || state == States.Playback)
        {
            Submarine.TurboAccelerate();
        }
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
