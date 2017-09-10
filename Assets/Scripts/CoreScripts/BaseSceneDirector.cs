using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSceneDirector : MonoBehaviour {

    protected Vector3 startPosition;
    protected Vector3 startRotation;
    protected Submarine.State startMode;

    protected Submarine submarine;
    
    /// <summary>
    /// Initialise startPosition, startRotation, startMode
    /// </summary>
    protected abstract void InitialiseStartVariables();

    private void Awake ()
    {
        submarine = FindObjectOfType<Submarine>();
        InitialiseStartVariables();
        ResetSubmarinePosition();
	}

    public void ResetSubmarinePosition()
    {
        submarine.transform.position = startPosition;
        submarine.transform.eulerAngles = startRotation;
        submarine.GetComponent<Rigidbody>().velocity = Vector3.zero;
        submarine.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        submarine.SetSubmarineState(startMode);
    }
}
