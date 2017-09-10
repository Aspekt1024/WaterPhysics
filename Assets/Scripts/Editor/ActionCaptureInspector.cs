using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

[CustomEditor(typeof(ActionCapture))]
public class ActionCaptureInspector : Editor {

    private ActionCapture ac;

    private bool savePressed;
    private bool playbackPressed;
    private bool isRecording;

    private int sessionIndex;
    private string newSessionName;

    public override void OnInspectorGUI()
    {
        ac = (ActionCapture)target;

        ShowRecordGUI();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        // TODO don't display when game is not running
        ShowSaveGUI();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        ShowPlaybackGUI();
    }

    private void ShowRecordGUI()
    {
        if (isRecording)
        {
            if (GUILayout.Button("End Recording"))
            {
                isRecording = false;
                ActionCapture.StopRecording();
            }
        }
        else
        {
            if (GUILayout.Button("Start Recording"))
            {
                isRecording = true;
                ActionCapture.StartRecording();
            }
        }
    }

    private void ShowPlaybackGUI()
    {
        ac.SessionName = GetSessionName();
        if (ac.SessionName == "") return;
        if (playbackPressed)
        {
            EditorGUILayout.LabelField("Playback " + ac.SessionName + "?");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Yes"))
            {
                savePressed = false;
                playbackPressed = false;
                ActionPlayback.PlayBack(ac.SessionName);
            }
            if (GUILayout.Button("No"))
            {
                playbackPressed = false;
            }
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            if (GUILayout.Button("Playback"))
            {
                playbackPressed = true;
            }
        }
    }
    
    private string GetSessionName()
    {
        var info = new DirectoryInfo(Application.dataPath + "/Resources/ActionSaves");
        var sessionNames = from file in info.GetFiles()
                           where file.Name.EndsWith(".json")
                           select Path.GetFileNameWithoutExtension(file.Name);

        string[] sessions = sessionNames.ToArray();
        if (sessions.Length == 0) return "";

        sessionIndex = EditorGUILayout.Popup("Session to load:", sessionIndex, sessions);
        return sessions[sessionIndex];
    }

    private void ShowSaveGUI()
    {
        newSessionName = EditorGUILayout.TextField("New Session Name", newSessionName);
        if (savePressed)
        {
            EditorGUILayout.LabelField("Overwrite " + newSessionName + "?");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Overwrite"))
            {
                ActionCapture.Save(newSessionName);
                savePressed = false;
            }
            if (GUILayout.Button("Cancel"))
            {
                savePressed = false;
            }
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            if (GUILayout.Button("Save"))
            {
                savePressed = true;
            }
        }
    }
}
