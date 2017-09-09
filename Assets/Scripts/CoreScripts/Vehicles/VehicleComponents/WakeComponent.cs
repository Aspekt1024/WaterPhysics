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
        switch (SubmarineReference.GetSubmarineState())
        {
            case Submarine.State.Floating:
                SetFloatingWake();
                break;
            case Submarine.State.Periscope:
                SetPeriscopeWake();
                break;
            case Submarine.State.Submerged:
                SetSubmergedWake();
                break;
            default:
                break;
        }
    }
    
    private void SetFloatingWake()
    {
        if (!WakeToBeSet()) return;
        SetWakePosition(0f, 40f);
        SetEmissionBasedOnSpeed();
        SetWakeSize(4.85f);
    }

    private void SetPeriscopeWake()
    {
        if (!WakeToBeSet()) return;
        
        SetWakePosition(1.98f, 15f);
        SetEmissionBasedOnSpeed();
        SetWakeSize(0.0f);
    }

    private void SetSubmergedWake()
    {
        if (!WakeToBeSet()) return;

        SetWakePosition(0f, 1f);
        SetEmissionBasedOnSpeed();
        SetWakeSize(.1f);
    }

    private void SetWakePosition(float distance, float angle)
    {
        float yPos = WaterControl.GetWaveYPos(new Vector3(FloatingWakePoint.position.x, 0f, FloatingWakePoint.position.z));
        WakeEffectTf.position = new Vector3(FloatingWakePoint.position.x, yPos, FloatingWakePoint.position.z);
        Vector3 lookTargetPosition = transform.position + transform.up * 10;
        lookTargetPosition.y = 0.2f;
        WakeEffectTf.LookAt(lookTargetPosition);
        WakeEffectTf.eulerAngles = new Vector3(90, wakeEffect.transform.eulerAngles.y, 90f - angle / 2f);

        Vector3 offset = new Vector3(WakeEffectTf.forward.x, 0f, WakeEffectTf.forward.z);
        WakeEffectTf.position +=  offset * distance;

        ParticleSystem.ShapeModule shape = wakeEffect.shape;
        shape.arc = angle;
    }

    private void SetEmissionBasedOnSpeed()
    {
        Vector3 velocity = body.velocity;
        velocity.y = 0;
        float speed = velocity.magnitude;

        ParticleSystem.EmissionModule emission = wakeEffect.emission;
        emission.rateOverTime = speed * 70f / 3f;

        ParticleSystem.MainModule main = wakeEffect.main;
        main.startSpeed = speed/2f;
    }

    private void SetWakeSize(float size)
    {
        ParticleSystem.MainModule main = wakeEffect.main;
        main.startSize = size;
    }

    private bool WakeToBeSet()
    {
        if (body.isKinematic)
        {
            wakeEffect.Stop();
            return false;
        }
        else
        {
            if (wakeEffect.isStopped)
            {
                wakeEffect.Play();
            }
            return true;
        }
    }
}
