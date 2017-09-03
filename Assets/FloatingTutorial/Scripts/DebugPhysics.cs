using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows us to change the water physics in real time
/// </summary>
public class DebugPhysics : MonoBehaviour {

    public static DebugPhysics current;

    [Header("Pressure Drag Force")]
    public float VelocityReference;

    [Header("Pressure Drag")]
    public float C_PD1 = 10f;
    public float C_PD2 = 10f;
    public float f_P = 0.5f;

    [Header("SuctionDrag")]
    public float C_SD1 = 10f;
    public float C_SD2 = 10f;
    public float f_S = 0.5f;

    [Header("Slamming Force")]
    public float RampUpSlammingForcePower = 2f;
    public float MaxAcceleration;
    public float SlammingCheat;

    private void Start()
    {
        current = this;
    }
}
