using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeComponent : MonoBehaviour {
    
    public Transform WakeEffectTf;
    public Transform FloatingWakePoint;
    public Transform PeriscopeWakePoint;
    public Submarine SubmarineReference;

    private ParticleSystem wakeEffect;
    private Rigidbody body;

    private void Start ()
    {
        wakeEffect = WakeEffectTf.GetComponent<ParticleSystem>();
        body = SubmarineReference.GetComponent<Rigidbody>();
	}
	
	private void Update ()
    {
        if (body.isKinematic)
        {
            wakeEffect.Stop();
            return;
        }
        else
        {
            if (wakeEffect.isStopped)
            {
                wakeEffect.Play();
            }
        }

        switch (SubmarineReference.GetSubmarineState())
        {
            case Submarine.State.Floating:
                SetFloatingWake();
                break;
            case Submarine.State.Periscope:
                SetPeriscopeWake();
                break;
            default:
                break;
        }
    }

    private void SetFloatingWake()
    {
        WakeEffectTf.position = new Vector3(FloatingWakePoint.position.x, WakeEffectTf.position.y, FloatingWakePoint.position.z);
        Vector3 lookTargetPosition = transform.position + transform.up * 10;
        lookTargetPosition.y = 0.2f;
        wakeEffect.transform.LookAt(lookTargetPosition);
        wakeEffect.transform.eulerAngles = new Vector3(90, wakeEffect.transform.eulerAngles.y, 70f);

        Vector3 velocity = body.velocity;
        velocity.y = 0;
        float speed = velocity.magnitude;
        
        ParticleSystem.EmissionModule emission = wakeEffect.emission;
        emission.rateOverTime = speed * 70f / 3f;
    }

    private void SetPeriscopeWake()
    {

    }
}
