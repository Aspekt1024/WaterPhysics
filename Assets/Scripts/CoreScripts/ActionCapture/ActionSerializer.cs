using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class ActionSerializer {

    private static string SAVE_PATH = "/Resources/ActionSaves";
    private static string LOAD_PATH = "ActionSaves";

    public static void Save(List<ActionType> actions, string sessionName)
    {
        ActionContainer actionData = new ActionContainer();
        actionData.Actions = actions;

        string path = string.Format("{0}/{1}.json", SAVE_PATH, sessionName);
        string json = JsonUtility.ToJson(actionData);
        File.WriteAllText(Application.dataPath + path, json);

#if UNITY_EDITOR
        AssetDatabase.ImportAsset("Assets" + path);
#endif

        Debug.Log(path + " saved!");
    }

    public static ActionContainer Load(string sessionName)
    {
        string path = LOAD_PATH + "/" + sessionName;
        TextAsset json = Resources.Load<TextAsset>(path);
        string jsonString = json.text;

        return JsonUtility.FromJson<ActionContainer>(jsonString);
    }
}
