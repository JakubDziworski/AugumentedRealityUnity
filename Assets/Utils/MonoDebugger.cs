using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class MonoDebugger : MonoBehaviour
{
    private IDictionary<string, string> consoleLog;

    #region SingletonBoilerPlate
    private static MonoDebugger mInstance;
    public static MonoDebugger Instance
    {
        get
        {
            if (mInstance == null)
            {
                var obj = new GameObject("MonoDebuggerHelper");
                mInstance = obj.AddComponent<MonoDebugger>();
                mInstance.Start();
            }
            return mInstance;
        }
    }
    #endregion SingletonBoilerPlate

    public void Awake()
    {
        consoleLog = new Dictionary<string, string>();
    }


    // Use this for initialization
    void Start()
    {
        
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
