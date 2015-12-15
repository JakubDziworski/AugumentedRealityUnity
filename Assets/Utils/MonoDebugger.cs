using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class MonoDebugger : Singleton<MonoDebugger>
{
    private IDictionary<string, string> consoleLog;

    public void Awake()
    {
        consoleLog = new Dictionary<string, string>();
    }

    public void printForKey(string key, string value)
    {
        if (consoleLog.ContainsKey(key))
        {
            consoleLog[key] = value;
        }
        else
        {
            consoleLog.Add(key, value);
        }
    }

    // Update is called once per frame
    void OnGUI()
    {
        StringBuilder builder = new StringBuilder();
        foreach (var pair in consoleLog)
        {
            builder.AppendLine(pair.Key + " : " + pair.Value);
        }
        GUI.Label(new Rect(0, 0, 500, 500), builder.ToString());
    }
}
