using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance3SceneDirector : BaseSceneDirector
{
    protected override void InitialiseStartVariables()
    {
        startPosition = new Vector3(87.48f, -1.84f, 69.03f);
        startRotation = new Vector3(-90f, 0f, -81.80801f);
        startMode = Submarine.State.Periscope;
    }
}
