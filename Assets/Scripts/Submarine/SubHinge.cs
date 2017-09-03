using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubHinge : MonoBehaviour {
    
    public Transform HingeJoint;
    public Transform Hatch;

    public Transform Flag;
    
	void Start () {
		
	}
	
	private void Update () {
		if (Input.GetKeyUp("a"))
        {
            StartCoroutine(OpenHatch());
        }
	}

    private IEnumerator OpenHatch()
    {
        //float startY = 6f;
        //float endY = 10.5f;
        //float startX = 70f;
        //float endX = 57f;

        //Vector3 endPos = new Vector3(endX, endY, Submarine.position.z);
        //Vector3 startPos = new Vector3(startX, startY, Submarine.position.z);
        
        float totalRotation = 120f;
        while (totalRotation > 1f)
        {
            float rotationAmount = Time.deltaTime * 360f;
            totalRotation -= rotationAmount;
            Hatch.RotateAround(HingeJoint.position, HingeJoint.right, -rotationAmount);
            yield return null;
        }

        float flagRaiseDistance = 3f;
        while (flagRaiseDistance > 0.03f)
        {
            float raiseDist = Mathf.Lerp(0, flagRaiseDistance, Time.deltaTime * 2);
            flagRaiseDistance -= raiseDist;
            Flag.position += Flag.transform.up * raiseDist;
            yield return null;
        }
    }
}
