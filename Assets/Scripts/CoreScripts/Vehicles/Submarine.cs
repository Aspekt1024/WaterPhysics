using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submarine : MonoBehaviour, IHasBuoyancy, ISubmergable, IHasSpecial, IHasMovement {

    public enum State
    {
        None, Floating, Periscope, Submerged
    }
    private State state;

    private BuoyancyComponent buoyancyComponent;
    private MovementComponent movementComponent;
    private SubmarineSpecial submarineSpecial;

    #region Lifecycle
    private void Start ()
    {
        state = State.Floating;
        GetSubmarineComponents();
        SetBounceDampening(0.4f);
        buoyancyComponent.SetEnabled();
	}
	
	private void Update ()
    {
		switch (state)
        {
            case State.None:
                break;
            case State.Floating:
                break;
            case State.Periscope:
                break;
            case State.Submerged:
                break;
        }
	}
    #endregion

    public State GetSubmarineState()
    {
        return state;
    }

    #region Interface Methods

    public void SetBounceDampening(float dampening)
    {
        buoyancyComponent.BounceDampening = dampening;
    }

    public void DisableBuoyancy() { buoyancyComponent.SetDisabled(); }
    public void EnableBuoyancy() { buoyancyComponent.SetEnabled(); }
    public void ToggleSpecial() { submarineSpecial.ToggleSpecial(); }

    public bool SetFloatMode() {
        state = State.Floating;
        return true;
    }

    public bool SetPeriscopeMode()
    {
        state = State.Periscope;
        return true;
    }

    public bool SetSubmergedMode()
    {
        state = State.Submerged;
        return true;
    }

    public bool Accelerate()
    {
        movementComponent.Accelerate();
        return true;
    }

    public bool TurnLeft()
    {
        movementComponent.TurnLeft();
        return true;
    }

    public bool TurnRight()
    {
        movementComponent.TurnRight();
        return true;
    }

    #endregion

    private void GetSubmarineComponents()
    {
        buoyancyComponent = GetComponent<BuoyancyComponent>();
        if (buoyancyComponent == null)
        {
            Debug.LogError(name + " requires a BuoyancyComponent");
        }

        movementComponent = GetComponent<MovementComponent>();
        if (movementComponent == null)
        {
            Debug.LogError(name + " requires a MovementComponent");
        }

        submarineSpecial = GetComponent<SubmarineSpecial>();
    }
}
