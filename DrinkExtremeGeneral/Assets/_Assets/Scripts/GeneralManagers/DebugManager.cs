using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager
{
    public static DebugManager instance;
    public DebugManager()
    {
        Debug.Log("Initializing a new DebugManager.");
        if (instance == null)
        {
            instance = this;
            Debug.Log("DebugManager initialization successful.");
        }
        else
        {
            Debug.Log("DebugManager already exists. New initialization unsuccessful.");
        }
    }
}
