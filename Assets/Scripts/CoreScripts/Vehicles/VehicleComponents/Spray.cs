using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spray : MonoBehaviour {

    public Transform TopPoint;
    public Transform BottomPoint;
    public Transform SprayTf;
    
    private ParticleSystem sprayParticles;

    private Vector3 probeDirection;
    private float probeAngle;

    private float prevMagnitude;
    private bool splashTriggered;
    private Rigidbody submarineBody;
    private AudioSource audioSource;

    private AudioClip splash;
        
    private void Start ()
    {
        submarineBody = FindObjectOfType<Submarine>().GetComponent<Rigidbody>();
        audioSource = submarineBody.GetComponent<AudioSource>();
        sprayParticles = SprayTf.GetComponentInChildren<ParticleSystem>();
        sprayParticles.Stop();

        splash = Resources.Load<AudioClip>("Audio/watersplash");
    }
	
	private void Update ()
    {
        float waterHeight = WaterControl.GetWaveYPos(SprayTf.position);
        SetSprayPos(waterHeight);
    }
    
    private void SetSprayPos(float height)
    {
        probeDirection = Vector3.Normalize(TopPoint.position - BottomPoint.position);
        probeAngle = Mathf.Asin(probeDirection.y / probeDirection.magnitude);
        float yPos = height - BottomPoint.position.y + 0.5f * Time.deltaTime;
        float magnitude = Mathf.Clamp(yPos * Mathf.Sin(probeAngle), 0f, Vector3.Distance(TopPoint.position, BottomPoint.position) + 1f);

        if (magnitude > prevMagnitude + 1f * Time.deltaTime && magnitude < Vector3.Distance(TopPoint.position, BottomPoint.position))
        {
            if (!sprayParticles.isPlaying)
            {
                if (submarineBody.velocity.y < -7f)
                {
                    StartCoroutine(SplashRoutine());
                }
                else
                {
                    sprayParticles.Play();
                }
            }
        }
        else if (!splashTriggered)
        {
            sprayParticles.Stop();
        }
        prevMagnitude = magnitude;
        SprayTf.position = BottomPoint.position + magnitude * probeDirection;
    }


    private IEnumerator SplashRoutine()
    {
        const float splashDuration = 0.7f;
        
        splashTriggered = true;
        sprayParticles.Play();
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(splash);
        }
        yield return new WaitForSeconds(splashDuration);
        sprayParticles.Stop();
        ParticleSystem.EmissionModule emission = sprayParticles.emission;
        emission.enabled = false;
        splashTriggered = false;
    }
}
