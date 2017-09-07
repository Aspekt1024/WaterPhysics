using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDirectorManager : MonoBehaviour {
    
    public enum StartModes
    {
        None, Falling, Entrance1, Entrance2, Entrance3, Exit1, Exit2, Exit3
    }
    
    private static SceneDirectorManager sceneDirectorManager;
    public static SceneDirectorManager Instance
    {
        get
        {
            if (sceneDirectorManager == null)
            {
                Debug.LogError("No SceneDirectorManager found. Add one to the scene!");
            }
            else
            {
                return sceneDirectorManager;
            }
            return null;
        }
    }
    
    private void Start()
    {
        if (sceneDirectorManager != null)
        {
            Debug.LogError("Multiple SceneDirectorManagers detected in scene");
            enabled = false;
        }
        else
        {
            sceneDirectorManager = this;
        }
    }
    
    public void ResetToMode(StartModes mode)
    {
        BaseSceneDirector director = null;
        switch (mode)
        {
            case StartModes.None:
                return;
            case StartModes.Falling:
                director = GetSceneDirector<FallingSceneDirector>();
                break;
            case StartModes.Entrance1:
                director = GetSceneDirector<Entrance1SceneDirector>();
                break;
            case StartModes.Entrance2:
                director = GetSceneDirector<Entrance1SceneDirector>();
                break;
            case StartModes.Entrance3:
                director = GetSceneDirector<Entrance1SceneDirector>();
                break;
            case StartModes.Exit1:
                director = GetSceneDirector<Exit1SceneDirector>();
                break;
            case StartModes.Exit2:
                director = GetSceneDirector<Exit2SceneDirector>();
                break;
            case StartModes.Exit3:
                director = GetSceneDirector<Exit3SceneDirector>();
                break;
            default:
                return;
        }
        director.ResetSubmarinePosition();
    }

    private BaseSceneDirector GetSceneDirector<T>() where T : BaseSceneDirector
    {
        BaseSceneDirector director = FindObjectOfType<T>();
        if (director == null)
        {
            director = gameObject.AddComponent<T>();
        }
        return director;
    }
}
