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

    private float prevMagnitude;

    private AudioClip splash;
        
    private void Start ()
    {
        waterControl = FindObjectOfType<WaterControl>();
        sprayParticles = SprayTf.GetComponentInChildren<ParticleSystem>();
        sprayParticles.Stop();

        splash = Resources.Load<AudioClip>("Audio/watersplash");
    }
	
	private void Update ()
    {
        float waterHeight = waterControl.GetWaveYPos(SprayTf.position);
        SetSprayPos(waterHeight);
    }


    private void SetSprayPos(float height)
    {
        probeDirection = Vector3.Normalize(TopPoint.position - BottomPoint.position);
        probeAngle = Mathf.Asin(probeDirection.y / probeDirection.magnitude);
        float yPos = height - BottomPoint.position.y + 0.5f * Time.deltaTime;
        float magnitude = Mathf.Clamp(yPos * Mathf.Sin(probeAngle), 0f, Vector3.Distance(TopPoint.position, BottomPoint.position) + 1f);

        if (magnitude > prevMagnitude + 15f * Time.deltaTime && magnitude < Vector3.Distance(TopPoint.position, BottomPoint.position))
        {
            if (!sprayParticles.isPlaying)
            {
                StartCoroutine(SprayForSeconds(0.7f));
            }
        }
        else
        {
            //sprayParticles.Stop();
        }
        prevMagnitude = magnitude;
        SprayTf.position = BottomPoint.position + magnitude * probeDirection;
    }

    private bool isTriggered;

    private IEnumerator SprayForSeconds(float seconds)
    {
        if (isTriggered) yield break;
        isTriggered = true;
        sprayParticles.Play();
        AudioSource a = FindObjectOfType<Submarine>().GetComponentInChildren<AudioSource>();
        if (!a.isPlaying)
        {
            a.PlayOneShot(splash);
        }
        yield return new WaitForSeconds(seconds);
        sprayParticles.Stop();
        ParticleSystem.EmissionModule emission = sprayParticles.emission;
        emission.enabled = false;
    }
}
