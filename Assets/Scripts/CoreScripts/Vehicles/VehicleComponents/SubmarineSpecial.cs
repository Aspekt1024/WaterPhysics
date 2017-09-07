﻿using System.Collections;
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
            return;
        }
        else
        {
            specialActivated = true;
            StartCoroutine(SpecialAnimationRoutine());
        }
    }

    private IEnumerator SpecialAnimationRoutine()
    {
        yield return new WaitForSeconds(1.2f);

        float totalRotation = 120f;
        while (totalRotation > 1f)
        {
            float rotationAmount = Time.deltaTime * 360f;
            totalRotation -= rotationAmount;
            Hatch.RotateAround(HingeJoint.position, HingeJoint.right, -rotationAmount);
            yield return null;
        }

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
    }
}