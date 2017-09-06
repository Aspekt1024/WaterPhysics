using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour {
    
    public Transform RotationPoint;

    private Submarine submarine;
    private Rigidbody body;

    private const float acceleration = 3f;
    private const float turnRotation = 30f;
    
    private void Start()
    {
        submarine = FindObjectOfType<Submarine>();
        body = submarine.GetComponent<Rigidbody>();
    }

    public bool Accelerate()
    {
        body.AddForce(-transform.up * acceleration * body.mass);
        return true;
    }

    public bool TurnLeft()
    {
        transform.RotateAround(RotationPoint.position, Vector3.up, -turnRotation * Time.deltaTime);

        // TODO banking when i work out how to manipulate the rotation better
        // moving back to equilibrium is... tricky
        return true;
    }
    
    public bool TurnRight()
    {
        transform.RotateAround(RotationPoint.position, Vector3.up, turnRotation * Time.deltaTime);

        // TODO banking when i work out how to manipulate the rotation better
        // moving back to equilibrium is... tricky
        return true;
    }

}
