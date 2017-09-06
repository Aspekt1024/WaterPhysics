using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasMovement {
    bool TurnLeft();
    bool TurnRight();
    bool Accelerate();
}
