using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance2SceneDirector : BaseSceneDirector
{
    protected override void InitialiseStartVariables()
    {
        startPosition = new Vector3(76.31f, -1.84f, 94.96f);
        startRotation = new Vector3(-90f, 0f, -135.807f);
        startMode = Submarine.State.Periscope;
    }
}
