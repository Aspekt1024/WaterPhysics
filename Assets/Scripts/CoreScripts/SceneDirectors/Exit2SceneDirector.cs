using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit2SceneDirector : BaseSceneDirector
{
    protected override void InitialiseStartVariables()
    {
        startPosition = new Vector3(59.78146f, -9.02f, 73.34393f);
        startRotation = new Vector3(-90f, 0f, -83.035f);
        startMode = Submarine.State.Submerged;
    }
}
