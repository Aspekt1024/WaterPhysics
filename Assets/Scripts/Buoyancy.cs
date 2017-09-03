using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour {

    public Rigidbody Submarine;
    public float Upforce;
    public float DownForce;

    private void Start()
    {

    }

    private void FixedUpdate()
    {
            CalculateForce();
    }

    private void CalculateForce()
    {
        float force = 0f;
        if (Submarine.transform.position.y > 2)
        {
            force = Mathf.Pow(2f - Submarine.transform.position.y, 5) * DownForce;
        }
        else
        {
            force = -Mathf.Pow(Submarine.transform.position.y, 5) * Upforce;
        }
        if (Mathf.Sign(force) == Mathf.Sign(Submarine.velocity.y))
        {
            force -= 50 * Submarine.velocity.y;
        }
        Submarine.AddForceAtPosition(force * Vector3.up, Vector3.zero);
    }
}
