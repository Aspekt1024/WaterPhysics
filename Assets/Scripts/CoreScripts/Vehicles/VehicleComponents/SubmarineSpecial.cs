using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineSpecial : MonoBehaviour {
    
    public Transform HingeJoint;
    public Transform Hatch;
    public Transform Flag;
    
    private bool specialActivated;

    private void Start()
    {   
        Flag.gameObject.SetActive(false);
    }

    public void ToggleSpecial()
    {
        if (specialActivated)
        {
            specialActivated = false;
            StartCoroutine(ReverseSpecial());
            return;
        }
        else
        {
            specialActivated = true;
            StartCoroutine(ActivateSpecial());
        }
    }

    private IEnumerator ActivateSpecial()
    {
        float totalRotation = 120f;
        while (totalRotation > 1f)
        {
            float rotationAmount = Time.deltaTime * 360f;
            totalRotation -= rotationAmount;
            Hatch.RotateAround(HingeJoint.position, HingeJoint.right, -rotationAmount);
            yield return null;
        }
        Hatch.RotateAround(HingeJoint.position, HingeJoint.right, -totalRotation);

        Flag.gameObject.SetActive(true);
        Flag.GetComponentInChildren<Cloth>().externalAcceleration = new Vector3(3, 0, -1);
        float flagRaiseDistance = 3f;
        while (flagRaiseDistance > 2.4f)
        {
            float raiseDist = Mathf.Lerp(0, flagRaiseDistance, Time.deltaTime * 2);
            flagRaiseDistance -= raiseDist;
            Flag.position += Flag.transform.up * raiseDist;
            yield return null;
        }

        Flag.GetComponentInChildren<Cloth>().externalAcceleration = new Vector3(30, 0, -10);
        
        while (flagRaiseDistance > 0.1f)
        {
            float raiseDist = Mathf.Lerp(0, flagRaiseDistance, Time.deltaTime * 2);
            flagRaiseDistance -= raiseDist;
            Flag.position += Flag.transform.up * raiseDist;
            yield return null;
        }

        Flag.position += Flag.transform.up * flagRaiseDistance;
    }

    private IEnumerator ReverseSpecial()
    {
        Flag.GetComponentInChildren<Cloth>().externalAcceleration = new Vector3(3, 0, -1);

        float flagFallDistance = 4f;
        while (flagFallDistance > 1f)
        {
            float fallDist = Mathf.Lerp(0, flagFallDistance, Time.deltaTime * 2);
            flagFallDistance -= fallDist;
            Flag.position -= Flag.transform.up * fallDist;
            yield return null;
        }

        Flag.position -= Flag.transform.up * (flagFallDistance - 1f);
        Flag.gameObject.SetActive(false);

        float totalRotation = 120f;
        while (totalRotation > 1f)
        {
            float rotationAmount = Time.deltaTime * 360f;
            totalRotation -= rotationAmount;
            Hatch.RotateAround(HingeJoint.position, HingeJoint.right, rotationAmount);
            yield return null;
        }
        Hatch.RotateAround(HingeJoint.position, HingeJoint.right, totalRotation);
    }
}
