using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform ObjectToFollow;
	
	// Update is called once per frame
	void Update () {
        Vector3 targetPosition = ObjectToFollow.position + ObjectToFollow.up * 10 + Vector3.up * 5;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 2f);
        transform.LookAt(ObjectToFollow);
	}
}
