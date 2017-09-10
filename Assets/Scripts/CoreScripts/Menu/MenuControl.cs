using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour {

    public RectTransform MainMenuPanel;
    public RectTransform ControlsPanel;
    public RectTransform SequencePanel;

    private CanvasGroup mainCanvasGroup;

    private Vector3 activePanelPosition;
    private Vector3 inactivePanelPosition;

    private enum States
    {
        None, MainMenu, ControlsMenu, SequenceMenu
    }
    private States state;

    private GameController gameController;

    private void Awake()
    {
        mainCanvasGroup = GetComponent<CanvasGroup>();
        gameController = FindObjectOfType<GameController>();

        activePanelPosition = new Vector3(Screen.width / 2f, Screen.height / 2f);
        inactivePanelPosition = new Vector3(Screen.width * 2f, 0f, 0f);

        EnableMainMenu();
    }

    public void EnableMainMenu()
    {
        state = States.MainMenu;
        mainCanvasGroup.alpha = 1f;
        mainCanvasGroup.blocksRaycasts = true;
        GotoMainMenu();
    }

    public void DisableMainMenu()
    {
        state = States.None;
        mainCanvasGroup.alpha = 0f;
        mainCanvasGroup.blocksRaycasts = false;
    }

    public void PlayButtonPressed()
    {
        gameController.SetPlayingMode();
        DisableMainMenu();
    }

    public void ControlsButtonPressed()
    {
        state = States.ControlsMenu;
        GotoControlMenu();
    }

    public void SequenceButtonPressed()
    {
        state = States.SequenceMenu;
        GotoSequenceMenu();
    }

    public void MainMenuButtonPressed()
    {
        state = States.MainMenu;
        GotoMainMenu();
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
    }

    public bool MainMenuIsActive()
    {
        return state != States.None;
    }

    private void GotoMainMenu()
    {
        MainMenuPanel.position = activePanelPosition;
        ControlsPanel.position = inactivePanelPosition;
        SequencePanel.position = inactivePanelPosition;
    }

    private void GotoControlMenu()
    {
        MainMenuPanel.position = inactivePanelPosition;
        ControlsPanel.position = activePanelPosition;
        SequencePanel.position = inactivePanelPosition;
    }

    private void GotoSequenceMenu()
    {
        MainMenuPanel.position = inactivePanelPosition;
        ControlsPanel.position = inactivePanelPosition;
        SequencePanel.position = activePanelPosition;
    }
}
