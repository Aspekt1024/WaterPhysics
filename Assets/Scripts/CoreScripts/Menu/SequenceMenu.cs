using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceMenu : MonoBehaviour {

    private MenuControl menuControl;

    private void Awake()
    {
        menuControl = GetComponentInParent<MenuControl>();
        if (menuControl == null)
        {
            Debug.LogError("Could not find MenuControl in parent of SequenceMenu");
        }
    }

    public void EntranceExit1Pressed()
    {
        ActionPlayback.PlayBack("EntranceExit1");
        menuControl.DisableMainMenu();
    }

    public void EntranceExit2Pressed()
    {
        ActionPlayback.PlayBack("EntranceExit2");
        menuControl.DisableMainMenu();
    }

    public void TransitionPressed()
    {
        ActionPlayback.PlayBack("Transition");
        menuControl.DisableMainMenu();
    }
}
