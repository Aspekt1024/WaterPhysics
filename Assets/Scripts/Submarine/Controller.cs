using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    public Transform RotationPoint;
    public Transform WakeEffect;
    public Transform WakePoint;

    private float acceleration;
    private float speed;

    private ParticleSystem wakeEffect;
    

	// Use this for initialization
	void Start () {
        wakeEffect = WakeEffect.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        // TODO decouple
		if (Input.GetKeyDown("w"))
        {
            acceleration = 3f;
        }
        if (Input.GetKeyUp("w"))
        {
            acceleration = 0f;
        }
        
        Vector3 go = -transform.up * acceleration * GetComponent<Rigidbody>().mass;
        transform.GetComponent<Rigidbody>().AddForce(go);

        float rotation = 0f;
        float angularRotation = 0f;
        if (Input.GetKey("d"))
        {
            angularRotation = 10f;
            rotation = 30f;
        }
        if (Input.GetKey("a"))
        {
            angularRotation = -10f;
            rotation = -30f;
        }
        

        transform.RotateAround(RotationPoint.position, Vector3.up, rotation * Time.deltaTime);
        transform.Rotate(Vector3.up, angularRotation * Time.deltaTime);

        WakeEffect.position = new Vector3(WakePoint.position.x, WakeEffect.position.y, WakePoint.position.z);
        Vector3 targetPosition = transform.position + transform.up * 10;
        targetPosition.y = 0f;
        wakeEffect.transform.LookAt(targetPosition);
        wakeEffect.transform.eulerAngles = new Vector3(90, wakeEffect.transform.eulerAngles.y, 70f);

        float speed = GetComponent<Rigidbody>().velocity.magnitude;

        ParticleSystem.EmissionModule emission = wakeEffect.emission;
        emission.rateOverTime = speed * 70f / 3f;

        //if (speed < 0.8f)
        //{
        //    wakeEffect.Stop();
        //}
        //else
        //{
        //    if (!wakeEffect.isPlaying)
        //    {
        //        wakeEffect.Play();
        //    }
        //    wakeEffect.startSpeed = speed;
        //}
	}
}
