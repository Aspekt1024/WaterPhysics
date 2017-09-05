using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spray : MonoBehaviour {

    public Transform TopPoint;
    public Transform BottomPoint;
    public Transform SprayTf;

    private WaterControl waterControl;
    private ParticleSystem sprayParticles;

    private Vector3 probeDirection;
    private float probeAngle;
    private float prevHeight;

    private float prevMagnitude;

    // Use this for initialization
    void Start () {
        waterControl = FindObjectOfType<WaterControl>();
        sprayParticles = SprayTf.GetComponentInChildren<ParticleSystem>();
        sprayParticles.Stop();

	}
	
	// Update is called once per frame
	void Update () {
        float waterHeight = waterControl.GetWaveYPos(SprayTf.position);
        SetSprayPos(waterHeight);
        
        prevHeight = SprayTf.position.y;

    }


    private void SetSprayPos(float height)
    {
        probeDirection = Vector3.Normalize(TopPoint.position - BottomPoint.position);
        probeAngle = Mathf.Asin(probeDirection.y / probeDirection.magnitude);
        float yPos = height - BottomPoint.position.y + 0.5f * Time.deltaTime;
        float magnitude = Mathf.Clamp(yPos * Mathf.Sin(probeAngle), 0f, Vector3.Distance(TopPoint.position, BottomPoint.position) + 1f);

        if (magnitude > prevMagnitude + 0.0f * Time.deltaTime && magnitude < Vector3.Distance(TopPoint.position, BottomPoint.position))
        {
            if (!sprayParticles.isPlaying)
            {
                sprayParticles.Play();
            }
        }
        else
        {
            sprayParticles.Stop();
        }
        prevMagnitude = magnitude;
        SprayTf.position = BottomPoint.position + magnitude * probeDirection;
    }
}
