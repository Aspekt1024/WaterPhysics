using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasBuoyancy {
    void SetBounceDampening(float dampening);
    void EnableBuoyancy();
    void DisableBuoyancy();
}
