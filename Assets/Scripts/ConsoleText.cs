using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ConsoleText : MonoBehaviour {

    Text text;
    List<string> logs = new List<string>();
    int logCount;

	// Use this for initialization
	void Awake () {
        GameObject.DontDestroyOnLoad(transform.parent.gameObject);

        text = GetComponent<Text>();
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        SetLog(logString, "");
    }

    public void SetLog(string log,string stackTrack="")
    {
        if (logs.Count >= 20)
        {
            logs.RemoveAt(0);
        }
        logs.Add(log+" ("+stackTrack+")");

        text.text = "";
        foreach(string str in logs)
        {
            text.text += str + "\n";
        }
    }
}
