﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit1SceneDirector : BaseSceneDirector
{
    protected override void InitialiseStartVariables()
    {
        startPosition = new Vector3(59.78146f, -0.73f, 73.34393f);
        startRotation = new Vector3(-90f, 0f, -83.035f);
        startMode = Submarine.State.Floating;
    }
}
